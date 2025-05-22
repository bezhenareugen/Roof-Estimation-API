using System.Security.Claims;
using RoofEstimation.Models.Estimation;

namespace RoofEstimation.BLL.Services.EstimationService.Calculation;

public interface IEstimationCalculationService
{
    Task<Estimation> CalculateTotal(ClaimsPrincipal authUser, EstimateRequest estimateRequest);
}