using System.Security.Claims;
using RoofEstimation.Models.Estimation;

namespace RoofEstimation.BLL.Services.EstimationService;

public interface IEstimationService
{
    Task<Estimation> CalculateTotal(ClaimsPrincipal authUser, EstimateRequest estimateRequest);
}