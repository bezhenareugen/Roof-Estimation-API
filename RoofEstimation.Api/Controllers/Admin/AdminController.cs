using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoofEstimation.DAL;
using RoofEstimation.Entities.Auth;

namespace RoofEstimation.Api.Controllers.Admin;
[Authorize(Roles = "Admin")]
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

    [HttpGet("getAllUsers")]
    public async Task<IActionResult> GetAllUsers([FromQuery] string userType)
    {
        var users = new List<UserEntity>();
        switch (userType)
        {
            case "all":
                users = await applicationDbContext.Users.ToListAsync();
                break;
            case "admin":
            {
                var adminUsers = await userManager.GetUsersInRoleAsync("Admin");
                users = adminUsers.ToList();
                break;
            }
            case "blocked":
                users = await applicationDbContext.Users.Where(u => u.IsBlocked).ToListAsync();
                break;
        }

        return Ok(users);
    }
}

public class ChangeRoleRequest
{
    public string Email { get; set; }
    public string NewRole { get; set; }
}