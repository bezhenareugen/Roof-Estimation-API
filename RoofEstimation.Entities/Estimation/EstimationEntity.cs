using RoofEstimation.Entities.Labor;
using RoofEstimation.Entities.Material;
using RoofEstimation.Entities.PipeInfo;
using RoofEstimation.Entities.RoofInfo;
using RoofEstimation.Entities.TearOff;

namespace RoofEstimation.Entities.Estimation;

public class EstimationEntity
{
    public RoofInfoEntity? RoofInfo { get; set; }
    public PipeInfoEntity? PipeInfo { get; set; }
    public bool Gutters { get; set; } = true;
    public bool PermitFees { get; set; } = true;
    public bool IsBlocked { get; set; } = false;
    public DateTime RegisteredDateTime { get; set; }
    public bool IsEagleView { get; set; } = false;
    
    public IList<InstallLaborCostEntity> InstallLaborCosts { get; set; }
    public IList<MaterialEntity> Materials { get; set; }
    public IList<TearOffEntity> TearOffs { get; set; }
}