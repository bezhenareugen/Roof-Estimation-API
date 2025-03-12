using System.Security.Claims;
using RoofEstimation.Models.Auth;
using RoofEstimation.Models.Auth.Requests;

namespace RoofEstimation.BLL.Services.Auth;

public interface IAuthService
{
    Task<UserReponse> GetUserAsync(ClaimsPrincipal user);
    Task<AuthResultBase> RegisterAsync(UserRegistrationRequest userRegisterRequest);
    Task<AuthResultBase> LoginAsync(UserLoginRequest user);
    Task<AuthResultBase> RefreshTokenAsync(RefreshTokenRequest tokenRequest);
    Task<bool> ValidateEmailAsync(string email);
}