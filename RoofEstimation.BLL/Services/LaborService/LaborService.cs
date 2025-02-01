using System.Diagnostics.Contracts;
using RoofEstimation.Entities.Labor;
using RoofEstimation.Entities.RoofInfo;
using RoofEstimation.Models.Enums;

namespace RoofEstimation.BLL.Services.LaborService;

public class LaborService : ILaborService
{
    public static LaborCost GetCalculatedTearOffs(List<InstallLaborCostEntity> labors, RoofInfoEntity roofInfo)
    {
        var updatedLabors = labors.Select(l =>
        {
            var numberOfSquares = Calc(l.LaborType, roofInfo);
            return new InstallLaborCostEntity
            {
                LaborType = l.LaborType,
                Price = l.Price,
                Profit = l.Profit,
                NumberOfSquares = numberOfSquares,
                LaborPrice = numberOfSquares * l.Price,
                MyProfit = numberOfSquares * l.Profit
            };
        }).ToList();

        return new LaborCost(updatedLabors, updatedLabors.Sum(x => x.LaborPrice), updatedLabors.Sum(x => x.MyProfit));
    }

    private static decimal Calc(InstallContractorLabor laborType, RoofInfoEntity roofInfo)
    {
        var result = laborType switch
        {
            InstallContractorLabor.PitchUpTo6by12 => roofInfo.Squares,
            InstallContractorLabor.PitchFor7by12 => roofInfo.RoofPitchType == RoofPitchTypes.Type7by12 ? roofInfo.Squares : 0,
            InstallContractorLabor.PitchFor8by12 => roofInfo.RoofPitchType == RoofPitchTypes.Type8by12 ? roofInfo.Squares : 0,
            InstallContractorLabor.LowSlopeNumberOfSquares => roofInfo.LowSlope,
            InstallContractorLabor.UnderShot => roofInfo.UnderShots,
            InstallContractorLabor.Chimney => roofInfo.Chimneys,
            InstallContractorLabor.ACUntiCurb => roofInfo.ACCurb,
            InstallContractorLabor.AcUnitFrame => roofInfo.ACFrame,
            InstallContractorLabor.Skylights => roofInfo.SkyLights,
            InstallContractorLabor.SatelliteDishes => roofInfo.Dishes,
            InstallContractorLabor.Vallesyes => Math.Ceiling(roofInfo.ValleysLF / 9m),
            InstallContractorLabor.RidgeVentRoll9 => roofInfo.VentsRidge ? Math.Ceiling(roofInfo.VentsLF / 20m) : 0,
            InstallContractorLabor.DecoRidge10 => roofInfo.RidgeLF / 20m,
            InstallContractorLabor.LowProfileVent => roofInfo.Vents,
            InstallContractorLabor.PlyWoodCDXOSB => roofInfo.PlyWood ? (roofInfo.Squares * 3) * 1.05m : 0,
            InstallContractorLabor.Additional => 0,  // Placeholder for future logic
            InstallContractorLabor.Garbage => 1,  // Placeholder for future logic
            InstallContractorLabor.Other => 0,  // Placeholder for future logic
            //TODO: Review if we need MatTypes
            InstallContractorLabor.AddForWoodcrest => 0,
            InstallContractorLabor.AddForWoodmoor => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(laborType), laborType, "Invalid labor type provided.")
        };

        return Math.Ceiling(result);
    }
}