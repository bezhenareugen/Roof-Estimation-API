using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RoofEstimation.BLL.Mappers.Auth;
using RoofEstimation.BLL.Services.Auth;
using RoofEstimation.DAL;
using RoofEstimation.Entities.Auth;
using RoofEstimation.Entities.Enums;
using RoofEstimation.Models.Auth;
using RoofEstimation.Models.Auth.Requests;
using RoofEstimation.Models.Configs;

namespace RoofEstimation.Api.Controllers.Auth;

[ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpGet]
        [Route("User")]
        [Authorize]
        public async Task<UserReponse> GetUser()
        {
            return await authService.GetUserAsync(User);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest userRegisterRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await authService.RegisterAsync(userRegisterRequest);
                if (result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }

            return BadRequest(new UserRegistrationResponse
            {
                Errors = new List<string> { "Invalid payload" },
                Success = false
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {
                var result = await authService.LoginAsync(user);
                if (result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }

            return BadRequest(new UserRegistrationResponse
            {
                Errors = new List<string> { "Invalid payload" },
                Success = false
            });
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await authService.RefreshTokenAsync(tokenRequest);
                if (result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }

            return BadRequest(new UserRegistrationResponse
            {
                Errors = new List<string> { "Invalid payload" },
                Success = false
            });
        }

        [HttpGet("validateEmail")]
        public async Task<IActionResult> ValidateEmail([FromBody] ValidateEmailRequest request)
        {
            var emailExist = await authService.ValidateEmailAsync(request.Email);
            return Ok(emailExist);
        }
    }