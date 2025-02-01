using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RoofEstimation.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok("Roof Estimation Test Controller Works");
    }
}