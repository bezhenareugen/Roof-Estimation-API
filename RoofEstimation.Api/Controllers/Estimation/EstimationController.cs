using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoofEstimation.BLL.Services.EstimationService;
using RoofEstimation.BLL.Services.EstimationService.Calculation;
using RoofEstimation.Models.Estimation;

namespace RoofEstimation.Api.Controllers.Estimation;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EstimationController(IEstimationCalculationService estimationCalculationService, IEstimationService estimationService ) : ControllerBase
{
    [HttpGet("getUserAllEstimations")]
    public async Task<IActionResult> GetUserAllEstimations()
    {
        //var result = await estimationCalculationService.GetUserEstimationsAsync(User);
        
        return Ok();
    }
    
    [HttpPost("estimate")]
    public async Task<IActionResult> EstimateAsync([FromBody] EstimateRequest estimateRequest)
    {
        var result = await estimationCalculationService.CalculateTotal(User, estimateRequest);
        
        return Ok(result);
    }
}