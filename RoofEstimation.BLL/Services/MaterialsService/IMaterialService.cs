using RoofEstimation.Entities.Material;
using RoofEstimation.Entities.PipeInfo;
using RoofEstimation.Entities.RoofInfo;
using RoofEstimation.Models.Material;

namespace RoofEstimation.BLL.Services.MaterialsService;

public interface IMaterialService
{
    public MaterialResult CalculateMaterials(List<MaterialEntity> materials, RoofInfoEntity roofInfo, PipeInfoEntity pipeInfo);
}