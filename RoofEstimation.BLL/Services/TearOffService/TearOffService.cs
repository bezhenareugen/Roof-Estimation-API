using RoofEstimation.Entities.RoofInfo;
using RoofEstimation.Entities.TearOff;
using RoofEstimation.Models.Constants;
using RoofEstimation.Models.Enums;
using RoofEstimation.Models.TearOff;

namespace RoofEstimation.BLL.Services.TearOffService;

public class TearOffService : ITearOffService
{
    public TearOffResult GetCalculatedTearOffs(List<TearOffEntity> tearOffs, RoofInfoEntity roofInfo)
    {
        tearOffs.ForEach(x =>
        {
            x.NumberOfSquares = CalculateTearOffSquares(x.TearOffType, roofInfo);
            x.TearOffPrice = x.NumberOfSquares * x.Price;
            x.MyProfit = x.NumberOfSquares * x.Profit;
        });

        var total = decimal.Round(tearOffs.Sum(x => x.TearOffPrice), 2);

        return new TearOffResult(tearOffs, Math.Max(total, Constants.MinimalCharge)); // Ensuring minimal charge of 550
    }

    private static int CalculateTearOffSquares(TearOff tearOff, RoofInfoEntity roofInfo)
    {
        var tearOffMapping = new Dictionary<TearOff, Func<RoofInfoEntity, int>>
        {
            { TearOff.Layer4by12orTouchOn, c => IsPitchType(c, RoofPitchTypes.Type4by12) ? c.Squares : 0 },
            { TearOff.AdditionalLayer4by12, c => HasLayers(c, 2) && IsPitchType(c, RoofPitchTypes.Type4by12) ? c.Squares : 0 },
            { TearOff.AdditionalLayer4by12Second, c => HasLayers(c, 3) && IsPitchType(c, RoofPitchTypes.Type4by12) ? c.Squares : 0 },

            { TearOff.Layer6by12, c => IsPitchType(c, RoofPitchTypes.Type6by23) ? c.Squares : 0 },
            { TearOff.AdditionalLayer6by12, c => HasLayers(c, 2) && IsPitchType(c, RoofPitchTypes.Type6by23) ? c.Squares : 0 },
            { TearOff.AdditionalLayer6by12Second, c => HasLayers(c, 3) && IsPitchType(c, RoofPitchTypes.Type6by23) ? c.Squares : 0 },

            { TearOff.Layer7by12, c => IsPitchType(c, RoofPitchTypes.Type7by12) ? c.Squares : 0 },
            { TearOff.AdditionalLayer7by12, c => HasLayers(c, 2) && IsPitchType(c, RoofPitchTypes.Type7by12) ? c.Squares : 0 },
            { TearOff.AdditionalLayer7by12Second, c => HasLayers(c, 3) && IsPitchType(c, RoofPitchTypes.Type7by12) ? c.Squares : 0 },

            { TearOff.Layer8by12, c => IsPitchType(c, RoofPitchTypes.Type8by12) ? c.Squares : 0 },
            { TearOff.AdditionalLayer8by12, c => HasLayers(c, 2) && IsPitchType(c, RoofPitchTypes.Type8by12) ? c.Squares : 0 },
            { TearOff.AdditionalLayer8by12Second, c => HasLayers(c, 3) && IsPitchType(c, RoofPitchTypes.Type8by12) ? c.Squares : 0 },

            { TearOff.ExtraAmount, _ => 0 },  // Needs revision
            { TearOff.LowSlope, c => c.LowSlope },
            { TearOff.GroundDropOfSquares, _ => 0 },  // Needs revision
            { TearOff.GroundDropAdditionalLayer, _ => 0 }  // Needs revision
        };

        if (tearOffMapping.TryGetValue(tearOff, out var calculationFunc))
        {
            return calculationFunc(roofInfo);
        }

        throw new ArgumentOutOfRangeException(nameof(tearOff), tearOff, "Invalid TearOff type.");
    }

    private static bool IsPitchType(RoofInfoEntity roofInfo, RoofPitchTypes pitchType)
    {
        return roofInfo.RoofPitchType == pitchType;
    }

    private static bool HasLayers(RoofInfoEntity roofInfo, int minLayers)
    {
        return roofInfo.Layers >= minLayers;
    }
}