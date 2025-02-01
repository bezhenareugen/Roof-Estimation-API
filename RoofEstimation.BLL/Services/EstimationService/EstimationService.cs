// using Microsoft.EntityFrameworkCore;
// using RoofEstimation.BLL.Services.LaborService;
// using RoofEstimation.BLL.Services.MaterialsService;
// using RoofEstimation.BLL.Services.TearOffService;
// using RoofEstimation.DAL;
// using RoofEstimation.Models.Estimation;
// using RoofEstimation.Models.TearOff;
//
// namespace RoofEstimation.BLL.Services.EstimationService;
//
// public class EstimationService(
//     ApplicationDbContext context,
//     IMaterialService materialService,
//     ITearOffService tearOffsService,
//     ILaborService laborService)
//     : IEstimationService
// {
//     public MainTotalDto CalculateTotal(string id)
//     {
//         var user = context.Users.Include()
//         // var client = context.Users
//         //     .Include(x => x.RoofInfoEntity)
//         //     .Include(x => x.PipeInfoEntity)
//         //     .Include(x => x.Emails)
//         //     .FirstOrDefault(x => x.ClientEntityId.ToString() == id)
//         //     ?? throw new Exception("Client not found");
//
//         // client.IsApprovedByClient = false;
//         // context.Update(client);
//         // context.SaveChanges();
//
//         decimal guttersTotal = CalculateGuttersTotal(client);
//
//         var materials = context.Materials.ToList();
//         var tearOffs = context.TearOffs.ToList();
//         var labors = context.InstallLaborCostEntities.ToList();
//         var labor1 = laborService.GetCalculatedTearOffs(labors, client);
//         var tear = tearOffsService.GetCalculatedTearOffs(tearOffs, client);
//
//         decimal permitFees = client.PermitFees ? context.PermitFees.First().Price : 0;
//
//         var mainTotal = new Estimation
//         {
//             MaterialResult = materialService.CalculateMaterials(materials, client),
//             TearOffResult = tear,
//             GuttersTotal = guttersTotal,
//             LaborTotal = labor1.LaborTotal + labor1.LaborProfit,
//             PermitFees = permitFees,
//             EstimatedProfit = CalculateEstimatedProfit(tear, labor1)
//         };
//
//         var (woodcrestMaterial, woodmoorMaterial, durationMaterial) = CalculateMaterialTypes(mainTotal);
//         var (duration, woodcrest, woodmoor) = CalculateLaborCosts(client, labor1, durationMaterial);
//
//         return new MainTotalDto
//         {
//             Client = client,
//             MaterialsTotalDto = mainTotal.MaterialsTotalDto,
//             TearOffDto = mainTotal.TearOffDto,
//             GuttersTotal = mainTotal.GuttersTotal,
//             LaborTotal = mainTotal.LaborTotal,
//             PermitFees = mainTotal.PermitFees,
//             EstimatedProffit = mainTotal.EstimatedProffit,
//             Woodcrest = CalculateTotalWithWood(mainTotal, woodcrest, woodcrestMaterial, duration, durationMaterial),
//             Woodmoor = CalculateTotalWithWood(mainTotal, woodmoor, woodmoorMaterial, duration, durationMaterial)
//         };
//     }
//
//     private decimal CalculateGuttersTotal(ClientEntity client)
//     {
//         var gutterFeet = context.GuttersEntities.First();
//         gutterFeet.Units = client.RoofInfoEntity.GutterLF;
//
//         var downSpots = context.GuttersEntities.OrderByDescending(x => x.Id).First();
//         downSpots.Units = client.RoofInfoEntity.DownSpots;
//
//         return client.Gutters 
//             ? gutterFeet.Total + downSpots.Total + gutterFeet.MyProfit + downSpots.MyProfit
//             : 0;
//     }
//
//     private decimal CalculateEstimatedProfit(TearOffResult tear, LaborCalculation labor)
//     {
//         return tear.TearOffWithPrices.Sum(x => x.MyProfit) +
//                labor.InstallLaborCostEntities.Sum(x => x.MyProfit) +
//                context.GuttersEntities.Sum(x => x.MyProfit);
//     }
//
//     private (decimal woodcrestMaterial, decimal woodmoorMaterial, decimal durationMaterial) 
//         CalculateMaterialTypes(MainTotalDto mainTotal)
//     {
//         var calcMaterialsPrice = mainTotal.MaterialsTotalDto.Materials
//             .Where(x => x.MaterialType != MaterialsTypes.BundlesOfMaterials)
//             .Sum(x => x.CalcPrice);
//
//         var bundle = mainTotal.MaterialsTotalDto.Materials
//             .FirstOrDefault(x => x.MaterialType == MaterialsTypes.BundlesOfMaterials)
//             ?.MaterialUnits ?? 0;
//
//         decimal baseMultiplier = 1.085m;
//
//         return (
//             woodcrestMaterial: decimal.Round(((bundle * 180m) + calcMaterialsPrice) * baseMultiplier, 2),
//             woodmoorMaterial: decimal.Round(((bundle * 225m) + calcMaterialsPrice) * baseMultiplier, 2),
//             durationMaterial: decimal.Round(((bundle * 110m) + calcMaterialsPrice) * baseMultiplier, 2)
//         );
//     }
//
//     private (decimal duration, decimal woodcrest, decimal woodmoor) 
//         CalculateLaborCosts(ClientEntity client, LaborCalculation labor, decimal durationMaterial)
//     {
//         var duration = labor.InstallLaborCostEntities
//             .Where(x => x.LaborType != InstallContractorLabor.AddForWoodcrest &&
//                         x.LaborType != InstallContractorLabor.AddForWoodmoor)
//             .Sum(x => x.LaborPrice);
//
//         var woodcrest = duration + client.RoofInfoEntity.Squares *
//                         (labor.InstallLaborCostEntities.FirstOrDefault(x => x.LaborType == InstallContractorLabor.AddForWoodcrest)?.Price ?? 0);
//
//         var woodmoor = duration + client.RoofInfoEntity.Squares *
//                        (labor.InstallLaborCostEntities.FirstOrDefault(x => x.LaborType == InstallContractorLabor.AddForWoodmoor)?.Price ?? 0);
//
//         return (duration, woodcrest, woodmoor);
//     }
//
//     private decimal CalculateTotalWithWood(MainTotalDto mainTotal, decimal woodType, decimal woodMaterial, decimal duration, decimal durationMaterial)
//     {
//         return mainTotal.MaterialsTotalDto.TotalWithTax +
//                mainTotal.PermitFees +
//                mainTotal.LaborTotal +
//                mainTotal.GuttersTotal +
//                mainTotal.TearOffDto.Total +
//                mainTotal.TearOffDto.TearOffWithPrices.Sum(x => x.MyProfit) +
//                (woodType - duration) +
//                (woodMaterial - durationMaterial);
//     }
// }