using RoofEstimation.Entities.RoofInfo;

namespace RoofEstimation.Api.UnitTests;

public static class RoofInfosRealData
{
    public static List<RoofInfoEntity> RoofInfos { get; } =
    [
        new RoofInfoEntity
        {
            RoofInfoEntityId = 75,
            Squares = 30,
            LowSlope = 6,
            ValleysLF = 9,
            RidgeLF = 9,
            GaldesLF = 9,
            GutterLF = 7,
            GutterSize = 5,
            DownSpots = 89,
            RoofPitchType = 0,
            Vents = 2,
            VentsLF = 0,
            VentsCutIn = true,
            VentsRidge = false,
            StepFlashing = 0,
            PlyWood = false,
            RoofSlope = 0,
            Stories = 8,
            Layers = 4,
            UnderShots = 2,
            Chimneys = 1,
            ACUnits = 0,
            SkyLights = 8,
            StepFlashingLF = 9,
            Dishes = 2,
            ACCurb = 3,
            ACFrame = 4
        },
    ];
}