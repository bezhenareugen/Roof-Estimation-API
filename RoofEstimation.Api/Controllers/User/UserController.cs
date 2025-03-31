using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoofEstimation.Entities.Auth;
using RoofEstimation.Models.User;

namespace RoofEstimation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserManager<UserEntity> _userManager;

    public UserController(UserManager<UserEntity> userManager)
    {
        _userManager = userManager;
    }
    
    [HttpPatch("updateUser")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest updateUserRequest)
    {   
        
        if (!ModelState.IsValid)
        {
            return BadRequest(new { Errors = new List<string> { "Invalid payload" } });
        }

        var user = await _userManager.FindByEmailAsync(updateUserRequest.Email);
        if (user == null)
        {
            return NotFound(new { Errors = new List<string> { "User not found" } });
        }

        user.FirstName = updateUserRequest.FirstName ?? user.FirstName;
        user.LastName = updateUserRequest.LastName ?? user.LastName;
        user.PhoneNumber = updateUserRequest.Phone ?? user.PhoneNumber;
        user.Address = updateUserRequest.Address ?? user.Address;
        user.City = updateUserRequest.City ?? user.City;
        user.State = updateUserRequest.State ?? user.State;
        user.Zip = updateUserRequest.Zip != 0 ? updateUserRequest.Zip : user.Zip;
        user.CompanyName = updateUserRequest.CompanyName ?? user.CompanyName;
        user.LicenseNumber = updateUserRequest.LicenseNumber ?? user.LicenseNumber;
        user.CompanyType = updateUserRequest.CompanyType ?? user.CompanyType;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return Ok(new { Success = true });
        }

        return BadRequest(new { Errors = result.Errors.Select(e => e.Description).ToList() });
    }
}