using RoofEstimation.Models.Estimation;

namespace RoofEstimation.BLL.Services.EstimationService;

public interface IEstimationService
{
    Estimation CalculateTotal(string id);
}