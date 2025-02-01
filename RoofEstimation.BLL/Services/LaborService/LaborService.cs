using RoofEstimation.Entities.Labor;
using RoofEstimation.Entities.RoofInfo;
using RoofEstimation.Models.Enums;

namespace RoofEstimation.BLL.Services.LaborService;

public class LaborService : ILaborService
{
    private static readonly Dictionary<InstallContractorLabor, Func<RoofInfoEntity, decimal>> LaborCostCalculations =
        new()
        {
            { InstallContractorLabor.PitchUpTo6by12, c => c.Squares },
            { InstallContractorLabor.PitchFor7by12, c => IsPitchType(c, RoofPitchTypes.Type7by12) ? c.Squares : 0 },
            { InstallContractorLabor.PitchFor8by12, c => IsPitchType(c, RoofPitchTypes.Type8by12) ? c.Squares : 0 },
            { InstallContractorLabor.LowSlopeNumberOfSquares, c => c.LowSlope },
            { InstallContractorLabor.UnderShot, c => c.UnderShots },
            { InstallContractorLabor.Chimney, c => c.Chimneys },
            { InstallContractorLabor.ACUntiCurb, c => c.ACCurb },
            { InstallContractorLabor.AcUnitFrame, c => c.ACFrame },
            { InstallContractorLabor.Skylights, c => c.SkyLights },
            { InstallContractorLabor.SatelliteDishes, c => c.Dishes },
            { InstallContractorLabor.Vallesyes, c => CalculateCeilingDivided(c.ValleysLF, 9) },
            { InstallContractorLabor.RidgeVentRoll9, c => c.VentsRidge ? CalculateCeilingDivided(c.VentsLF, 20) : 0 },
            { InstallContractorLabor.DecoRidge10, c => c.RidgeLF / 20m },
            { InstallContractorLabor.LowProfileVent, c => c.Vents },
            { InstallContractorLabor.PlyWoodCDXOSB, CalculatePlywood },

            // Placeholder values for future logic
            { InstallContractorLabor.Additional, _ => 0 },
            { InstallContractorLabor.Garbage, _ => 1 },
            { InstallContractorLabor.Other, _ => 0 },
            { InstallContractorLabor.AddForWoodcrest, _ => 0 },
            { InstallContractorLabor.AddForWoodmoor, _ => 0 }
        };
    
    public LaborCost GetCalculatedTearOffs(List<InstallLaborCostEntity> labors, RoofInfoEntity roofInfo)
    {
        var updatedLabors = labors.Select(l => new InstallLaborCostEntity
        {
            LaborType = l.LaborType,
            Price = l.Price,
            Profit = l.Profit,
            NumberOfSquares = CalculateLaborCost(l.LaborType, roofInfo),
            LaborPrice = l.Price * CalculateLaborCost(l.LaborType, roofInfo),
            MyProfit = l.Profit * CalculateLaborCost(l.LaborType, roofInfo)
        }).ToList();

        return new LaborCost(updatedLabors, updatedLabors.Sum(x => x.LaborPrice), updatedLabors.Sum(x => x.MyProfit));
    }
    
    private static decimal CalculateLaborCost(InstallContractorLabor laborType, RoofInfoEntity roofInfo)
    {
        if (LaborCostCalculations.TryGetValue(laborType, out var calculationFunc))
        {
            return Math.Ceiling(calculationFunc(roofInfo));
        }

        throw new ArgumentOutOfRangeException(nameof(laborType), laborType, "Invalid labor type provided.");
    }

    private static bool IsPitchType(RoofInfoEntity roofInfo, RoofPitchTypes pitchType) => 
        roofInfo.RoofPitchType == pitchType;

    private static decimal CalculateCeilingDivided(decimal value, decimal divisor) => 
        Math.Ceiling(value / divisor);

    private static decimal CalculatePlywood(RoofInfoEntity roofInfo) => 
        roofInfo.PlyWood ? (roofInfo.Squares * 3) * 1.05m : 0;
}