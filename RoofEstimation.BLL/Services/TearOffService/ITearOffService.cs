using RoofEstimation.Entities.RoofInfo;
using RoofEstimation.Entities.TearOff;
using RoofEstimation.Models.TearOff;

namespace RoofEstimation.BLL.Services.TearOffService;

public interface ITearOffService
{
    public TearOffResult GetCalculatedTearOffs(List<TearOffEntity> tearOffs, RoofInfoEntity roofInfo);
}