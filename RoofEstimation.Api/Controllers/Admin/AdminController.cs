using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoofEstimation.DAL;
using RoofEstimation.Entities.Auth;

namespace RoofEstimation.Api.Controllers.Admin;

[ApiController]
[Route("api/[controller]")]
public class AdminController(UserManager<UserEntity> userManager, ApplicationDbContext applicationDbContext) : ControllerBase
{
    [HttpPost("chnageRole")]
    public async Task<IActionResult> ChangeRoleAsync([FromBody] ChangeRoleRequest changeRoleRequest)
    {
        var userExist = applicationDbContext.Users
            .FirstOrDefaultAsync(u => u.NormalizedEmail == changeRoleRequest.Email.ToUpper()).Result;

        if (userExist is not null)
        {
            await userManager.AddToRoleAsync(userExist, changeRoleRequest.NewRole);
        }
        
        return Ok();
    } 
}

public class ChangeRoleRequest
{
    public string Email { get; set; }
    public string NewRole { get; set; }
}