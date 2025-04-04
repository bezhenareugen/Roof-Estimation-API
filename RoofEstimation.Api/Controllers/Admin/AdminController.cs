using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoofEstimation.DAL;
using RoofEstimation.Entities.Admin;
using RoofEstimation.Entities.Auth;

namespace RoofEstimation.Api.Controllers.Admin;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController(UserManager<UserEntity> userManager, ApplicationDbContext applicationDbContext) : Controller
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
                users = await applicationDbContext.Users.Where(x => !x.IsBlocked).ToListAsync();
                break;
            case "admin":
            {
                var adminUsers = await userManager.GetUsersInRoleAsync("Admin");
                users = adminUsers.Where(x => !x.IsBlocked).ToList();
                break;
            }
            case "blocked":
                users = await applicationDbContext.Users.Where(u => u.IsBlocked).ToListAsync();
                break;
        }

        return Ok(users);
    }

    [HttpGet("searchUser")]
    public async Task<IActionResult> SearchUser([FromQuery] string search)
    {
        search ??= string.Empty;
        
        var users = await applicationDbContext.Users
            .Where(u => u.FirstName.Contains(search) ||
                        u.LastName.Contains(search) ||
                        u.Address.Contains(search) ||
                        u.City.Contains(search) ||
                        u.PhoneNumber!.Contains(search) ||
                        u.Email!.Contains(search) ||
                        u.Zip.ToString().Contains(search) ||
                        u.RegisteredDateTime.ToString().Contains(search))
            .ToListAsync();

        return Ok(users);
    }
    
    [HttpPost("blockUser")]
    public async Task<IActionResult> BlockUser([FromBody] BlockUserRequest blockUserRequest)
    {
        var userExist = applicationDbContext.Users
            .FirstOrDefaultAsync(u => u.NormalizedEmail == blockUserRequest.Email.ToUpper()).Result;

        if (userExist is null) return BadRequest("User not found");
        
        userExist.IsBlocked = true;
        await applicationDbContext.SaveChangesAsync();

        return Ok();
    }
    
    [HttpPost("unblockUser")]
    public async Task<IActionResult> UnblockUser([FromBody] BlockUserRequest blockUserRequest)
    {
        var userExist = applicationDbContext.Users
            .FirstOrDefaultAsync(u => u.NormalizedEmail == blockUserRequest.Email.ToUpper()).Result;

        if (userExist is null) return BadRequest("User not found");
        
        userExist.IsBlocked = false;
        await applicationDbContext.SaveChangesAsync();

        return Ok();
    }
}

public class ChangeRoleRequest
{
    public string Email { get; set; }
    public string NewRole { get; set; }
}