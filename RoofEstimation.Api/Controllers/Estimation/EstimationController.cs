using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoofEstimation.BLL.Services.EstimationService;
using RoofEstimation.Models.Estimation;

namespace RoofEstimation.Api.Controllers.Estimation;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EstimationController(IEstimationService estimationService) : ControllerBase
{
    [HttpPost("estimate")]
    public async Task<IActionResult> EstimateAsync([FromBody] EstimateRequest estimateRequest)
    {
        var result = await estimationService.CalculateTotal(User, estimateRequest);
        
        return Ok(result);
    }
}