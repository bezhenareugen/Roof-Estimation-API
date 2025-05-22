using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using RoofEstimation.BLL.Services.LaborService;
using RoofEstimation.BLL.Services.MaterialsService;
using RoofEstimation.BLL.Services.TearOffService;
using RoofEstimation.DAL;
using RoofEstimation.Entities.Auth;
using RoofEstimation.Entities.Enums;
using RoofEstimation.Entities.Estimation;
using RoofEstimation.Entities.Labor;
using RoofEstimation.Models.Enums;
using RoofEstimation.Models.Estimation;
using RoofEstimation.Models.TearOff;

namespace RoofEstimation.BLL.Services.EstimationService.Calculation;

public class EstimationCalculationCalculationService(
    UserManager<UserEntity> userManager,
    ApplicationDbContext context,
    IMaterialService materialService,
    ITearOffService tearOffsService,
    ILaborService laborService)
    : IEstimationCalculationService
{
    public async Task<Estimation> CalculateTotal(ClaimsPrincipal authUser, EstimateRequest estimateRequest)
    {
        var userId  = userManager.GetUserId(authUser);
        var user = await userManager.FindByEmailAsync(userId);

        if (user == null)
        {
            throw new Exception("User not found for estimation...");
        }

        if (estimateRequest.AdditionalAddress != null)
        {
            user.AdditionalAddresses?.Add(estimateRequest.AdditionalAddress);
        }

        var newEstimation = new EstimationEntity
        {
            RoofInfo = estimateRequest.RoofInfo,
            PipeInfo = estimateRequest.PipesInfo,
            
        };

        user.RoofInfo = estimateRequest.RoofInfo;
        user.PipeInfo = estimateRequest.PipesInfo;
        
        context.Users.Update(user); // check if this is correct and performance
        
        // var user = context.Users
        //     .Include(userEntity => userEntity.RoofInfo)
        //     .Include(userEntity => userEntity.PipeInfo)
        //     .SingleOrDefaultAsync(u => u.Id == id).Result
        //            ?? throw new Exception("Client not found");

        // client.IsApprovedByClient = false;
        // context.Update(client);
         await context.SaveChangesAsync();

        var guttersTotal = CalculateGuttersTotal(user);

        var materials = context.Materials.ToList();
        var tearOffs = context.TearOffs.ToList();
        var labors = context.InstallLaborCosts.ToList();
        var labor = laborService.GetCalculatedTearOffs(labors, user.RoofInfo);
        var tear = tearOffsService.GetCalculatedTearOffs(tearOffs, user.RoofInfo);

        var permitFees = user.PermitFees ? context.PermitFees.First().Price : 0;

        var mainTotal = new Estimation
        {
            MaterialResult = materialService.CalculateMaterials(materials, user.RoofInfo, user.PipeInfo),
            TearOffResult = tear,
            GuttersTotal = guttersTotal,
            LaborTotal = labor.LaborTotal + labor.LaborProfit,
            PermitFees = permitFees,
            EstimatedProfit = CalculateEstimatedProfit(tear, labor)
        };

        var (woodcrestMaterial, woodmoorMaterial, durationMaterial) = CalculateMaterialTypes(mainTotal);
        var (duration, woodcrest, woodmoor) = CalculateLaborCosts(user, labor, durationMaterial);

        return new Estimation
        {
            //Client = client,
            MaterialResult = mainTotal.MaterialResult,
            TearOffResult = mainTotal.TearOffResult,
            GuttersTotal = mainTotal.GuttersTotal,
            LaborTotal = mainTotal.LaborTotal,
            PermitFees = mainTotal.PermitFees,
            EstimatedProfit = mainTotal.EstimatedProfit,
            Woodcrest = CalculateTotalWithWood(mainTotal, woodcrest, woodcrestMaterial, duration, durationMaterial),
            Woodmoor = CalculateTotalWithWood(mainTotal, woodmoor, woodmoorMaterial, duration, durationMaterial)
        };
    }

    private decimal CalculateGuttersTotal(UserEntity user)
    {
        var gutterFeet = context.Gutters.First();
        gutterFeet.Units = user.RoofInfo!.GutterLF;

        var downSpots = context.Gutters.OrderByDescending(x => x.Id).First();
        downSpots.Units = user.RoofInfo.DownSpots;

        return user.Gutters 
            ? gutterFeet.Total + downSpots.Total + gutterFeet.MyProfit + downSpots.MyProfit
            : 0;
    }

    private decimal CalculateEstimatedProfit(TearOffResult tear, LaborCost labor)
    {
        return tear.TearOffWithPrices.Sum(x => x.MyProfit) +
               labor.LaborCosts.Sum(x => x.MyProfit) +
               context.Gutters.ToList().Sum(x => x.MyProfit);
    }

    private static (decimal woodcrestMaterial, decimal woodmoorMaterial, decimal durationMaterial) 
        CalculateMaterialTypes(Estimation estimation)
    {
        var calcMaterialsPrice = estimation.MaterialResult.Materials
            .Where(x => x.MaterialType != MaterialsType.BundlesOfMaterials)
            .Sum(x => x.CalcPrice);

        var bundle = estimation.MaterialResult.Materials
            .FirstOrDefault(x => x.MaterialType == MaterialsType.BundlesOfMaterials)
            ?.MaterialUnits ?? 0;

        const decimal baseMultiplier = 1.085m;

        return (
            woodcrestMaterial: decimal.Round(((bundle * 180m) + calcMaterialsPrice) * baseMultiplier, 2),
            woodmoorMaterial: decimal.Round(((bundle * 225m) + calcMaterialsPrice) * baseMultiplier, 2),
            durationMaterial: decimal.Round(((bundle * 110m) + calcMaterialsPrice) * baseMultiplier, 2)
        );
    }

    private static (decimal duration, decimal woodcrest, decimal woodmoor) 
        CalculateLaborCosts(UserEntity user, LaborCost labor, decimal durationMaterial) // check about duration material
    {
        var duration = labor.LaborCosts
            .Where(x => x.LaborType != InstallContractorLabor.AddForWoodcrest &&
                        x.LaborType != InstallContractorLabor.AddForWoodmoor)
            .Sum(x => x.LaborPrice);

        var woodcrest = duration + user.RoofInfo!.Squares *
                        (labor.LaborCosts.FirstOrDefault(x => x.LaborType == InstallContractorLabor.AddForWoodcrest)?.Price ?? 0);

        var woodmoor = duration + user.RoofInfo.Squares *
                       (labor.LaborCosts.FirstOrDefault(x => x.LaborType == InstallContractorLabor.AddForWoodmoor)?.Price ?? 0);

        return (duration, woodcrest, woodmoor);
    }

    private static decimal CalculateTotalWithWood(Estimation estimation, decimal woodType, decimal woodMaterial, decimal duration, decimal durationMaterial)
    {
        return estimation.MaterialResult.TotalWithTax +
               estimation.PermitFees +
               estimation.LaborTotal +
               estimation.GuttersTotal +
               estimation.TearOffResult.Total +
               estimation.TearOffResult.TearOffWithPrices.Sum(x => x.MyProfit) +
               (woodType - duration) +
               (woodMaterial - durationMaterial);
    }
}