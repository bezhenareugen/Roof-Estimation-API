using RoofEstimation.Entities.Enums;
using RoofEstimation.Entities.Material;
using RoofEstimation.Entities.PipeInfo;
using RoofEstimation.Entities.RoofInfo;
using RoofEstimation.Models.Material;

namespace RoofEstimation.BLL.Services.MaterialsService;

public class MaterialService : IMaterialService
{
    private static readonly Dictionary<MaterialsType, Func<RoofInfoEntity, decimal>> MaterialCalculations =
        new()
        {
            { MaterialsType.BundlesOfMaterials, r => r.Squares * 3 },
            { MaterialsType.TrebuildStarter, r => ((Math.Ceiling(r.ValleysLF / 9m) * 20m + r.GaldesLF + r.GutterLF)) / 100m },
            { MaterialsType.DecoRidge, r => r.RidgeLF / 20m },
            { MaterialsType.Nosing2x2, r => r.GaldesLF / 9.5m },
            { MaterialsType.Nosing3x2, r => r.GutterLF / 9.5m },
            { MaterialsType.SafeGuard, r => r.Squares / 4.5m },
            { MaterialsType.Felt30lb, r => Math.Ceiling(r.ValleysLF / 9m) / 3m },
            { MaterialsType.Valley, r => Math.Ceiling(r.ValleysLF / 9m) },
            { MaterialsType.RidgeVentRoll, r => r.VentsRidge ? Math.Ceiling(r.VentsLF / 20m) : 0 },
            { MaterialsType.OCLowProfile, r => r.Vents },
            { MaterialsType.StepFlashing, r => r.StepFlashingLF / 50m },
            { MaterialsType.PeelAndStickBase, r => Math.Ceiling(r.LowSlope / 2m) },
            { MaterialsType.PeelAndStickCap, r => r.LowSlope },
            { MaterialsType.Nails1by4, r => r.Squares / 20m },
            { MaterialsType.TackerStaples, r => r.Squares / 25m },
            { MaterialsType.PlyWood7by16OSB, r => r.PlyWood ? (r.Squares * 3) * 1.05m : 0 },
            { MaterialsType.StaplesN1911by2, r => r.PlyWood && (r.Squares * 3) * 1.05m > 1 ? 1 : 0 }
        };

    private static readonly Dictionary<MaterialsType, Func<PipeInfoEntity, decimal>> PipeCalculations =
        new()
        {
            { MaterialsType.ShakeBaseAndCollars1point5, p => p.ShakeBaseAndCollars1 },
            { MaterialsType.ShakeBaseAndCollars2, p => p.ShakeBaseAndCollars2 },
            { MaterialsType.ShakeBaseAndCollars3, p => p.ShakeBaseAndCollars3 },
            { MaterialsType.TTop4, p => p.TTop4 },
            { MaterialsType.HoodVent2Ppc4, p => p.HoodVent2Ppc4 },
            { MaterialsType.HoodVent2Ppc6, p => p.HoodVent2Ppc6 },
            { MaterialsType.TTop6, p => p.TTop6 },
            { MaterialsType.TTop7, p => p.TTop7 },
            { MaterialsType.HoodVent2Ppc8, p => p.HoodVent2Ppc8 },
            { MaterialsType.OvalVertical, p => p.OvalVertical },
            { MaterialsType.OvalHorizontal, p => p.OvalHorizontal },
            { MaterialsType.PipesOther, p => p.PipesOther },
            { MaterialsType.RoofToWalls, p => p.RoofToWalls },
            { MaterialsType.Nails3by4, p => p.Nails34 ? 1 : 0 },
            { MaterialsType.SiliconFlexiseal, p => 2 } // Should be revised
        };
    
    public MaterialResult CalculateMaterials(List<MaterialEntity> materials, RoofInfoEntity roofInfo, PipeInfoEntity pipeInfo)
    {
        materials.ForEach(x =>
        {
            x.MaterialUnits = (int)CalculateUnits(x.MaterialType, roofInfo, pipeInfo);
            x.CalcPrice = x.MaterialUnits * x.MaterialPrice;
        });

        var preTotal = decimal.Round(materials.Sum(x => x.CalcPrice), 2);
        var tax = preTotal * 0.0875m;

        return new MaterialResult(materials, preTotal, tax, preTotal + tax);
    }

    

    private static decimal CalculateUnits(MaterialsType materialType, RoofInfoEntity roofInfo, PipeInfoEntity pipeInfo)
    {
        if (MaterialCalculations.TryGetValue(materialType, out var roofCalculation))
        {
            return Math.Ceiling(roofCalculation(roofInfo));
        }

        if (PipeCalculations.TryGetValue(materialType, out var pipeCalculation))
        {
            return Math.Ceiling(pipeCalculation(pipeInfo));
        }

        if (materialType != MaterialsType.Paint)
            throw new ArgumentOutOfRangeException(nameof(materialType), materialType,
                "Invalid material type provided.");
        
        var sum = pipeInfo.ShakeBaseAndCollars1 +
                  pipeInfo.ShakeBaseAndCollars2 +
                  pipeInfo.ShakeBaseAndCollars3 +
                  pipeInfo.TTop4 +
                  pipeInfo.HoodVent2Ppc4 +
                  pipeInfo.HoodVent2Ppc6 +
                  pipeInfo.TTop6 +
                  pipeInfo.TTop7 +
                  pipeInfo.HoodVent2Ppc8 +
                  pipeInfo.OvalVertical +
                  pipeInfo.OvalHorizontal +
                  pipeInfo.PipesOther +
                  Math.Ceiling(roofInfo.StepFlashingLF / 50m);
        
        return Math.Ceiling(sum / 5m);
    }
}