using RoofEstimation.Entities.Labor;
using RoofEstimation.Entities.RoofInfo;

namespace RoofEstimation.BLL.Services.LaborService;

public interface ILaborService
{ 
    LaborCost GetCalculatedTearOffs(List<InstallLaborCostEntity> labors, RoofInfoEntity roofInfo);
}