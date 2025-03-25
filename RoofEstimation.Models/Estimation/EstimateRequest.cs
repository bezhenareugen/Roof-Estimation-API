using RoofEstimation.Entities.Auth;
using RoofEstimation.Entities.PipeInfo;
using RoofEstimation.Entities.RoofInfo;

namespace RoofEstimation.Models.Estimation;

public class EstimateRequest
{
    public AdditionalAddressEntity? AdditionalAddress { get; set; }
    public RoofInfoEntity RoofInfo { get; set; }
    public PipeInfoEntity PipesInfo { get; set; }
}