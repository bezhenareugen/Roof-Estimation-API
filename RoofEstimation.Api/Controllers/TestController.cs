using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoofEstimation.DAL;

namespace RoofEstimation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {    
        var test = context.Users.FirstOrDefault();
        return Ok(test);
    }
}