using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RoofEstimation.BLL.Mappers.Auth;
using RoofEstimation.BLL.Services.MailService;
using RoofEstimation.DAL;
using RoofEstimation.Entities;
using RoofEstimation.Entities.Auth;
using RoofEstimation.Entities.Enums;
using RoofEstimation.Models.Auth;
using RoofEstimation.Models.Auth.Requests;
using RoofEstimation.Models.Configs;
using RoofEstimation.Models.Emails;
using RoofEstimation.Models.Enums;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using ResetPasswordRequest = RoofEstimation.Models.Auth.Requests.ResetPasswordRequest;

namespace RoofEstimation.BLL.Services.Auth;

public class AuthService(
    UserManager<UserEntity> userManager,
    RoleManager<IdentityRole> roleManager,
    IOptionsMonitor<JwtConfig> optionsMonitor,
    ApplicationDbContext context,
    IMailService mailService)
    : IAuthService
{
    private readonly JwtConfig _jwtConfig = optionsMonitor.CurrentValue;

    public async Task<UserResponse> GetUserAsync(ClaimsPrincipal user)
    {   
        var userId  = userManager.GetUserId(user);
        var authUser = await userManager.FindByEmailAsync(userId);
        var roles = await userManager.GetRolesAsync(authUser);

        if (authUser == null)
        {
            throw new Exception();
        }
        
        var response = UserEntityToUserResponseMapper.MapToUserResponse(authUser);
        response.Roles = roles;

        return response;
    }

    public async Task<AuthResultBase> RegisterAsync(UserRegistrationRequest userRegisterRequest)
    {
        var isCompanyRoleExistsAsync = await roleManager.RoleExistsAsync("Company");

        if (!isCompanyRoleExistsAsync)
        {
            var role = new IdentityRole { Name = "Company" };
            await roleManager.CreateAsync(role);
        }

        var userRole = userRegisterRequest.UserType == UserType.Client ? "Client" : "Company";
        //var userRole = "Admin";
        var newUser = UserRegisterReqToUserEntityMapper.MapToUserEntity(userRegisterRequest);
        var isCreated = await userManager.CreateAsync(newUser, userRegisterRequest.Password);

        if (isCreated.Succeeded)
        {
            await userManager.AddToRoleAsync(newUser, userRole);
            
            // Store the password in the old passwords table
            var oldPasswordEntity = new OldPasswordEntity
            {
                UserId = newUser.Id,
                PasswordHash = userManager.PasswordHasher.HashPassword(newUser, userRegisterRequest.Password),
                CreatedAt = DateTime.UtcNow
            };

            context.OldPasswords.Add(oldPasswordEntity);
            await context.SaveChangesAsync();
            
            var result = await GenerateJwtToken(newUser);

            if (result.Success)
            {
                var sendEmailParams = new SendEmail
                {
                    ToName = $"{newUser.FirstName} {newUser.LastName}",
                    ToEmailAddress = newUser.Email,
                    Subject = "Welcome to RoofEst",
                    Body = string.Empty,
                    EmailType = EmailType.WelcomeEmail,
                };
                await mailService.SendEmailAsync(sendEmailParams);
            }

            return result;
        }

        return new AuthResultBase
        {
            Errors = isCreated.Errors.Select(x => x.Description).ToList(),
            Success = false,
        };
    }

    public async Task<AuthResultBase> LoginAsync(UserLoginRequest user)
    {
        var existingUser = await userManager.FindByEmailAsync(user.Email);

        if (existingUser == null || existingUser.IsBlocked)
        {
            return new AuthResultBase
            {
                Errors = ["User doesn't exist, or blocked"],
                Success = false
            };
        }

        var isCorrect = await userManager.CheckPasswordAsync(existingUser, user.Password);

        if (!isCorrect)
        {
            return new AuthResultBase
            {
                Errors = ["Password is incorrect"],
                Success = false
            };
        }

        return await GenerateJwtToken(existingUser);
    }

    public async Task<AuthResultBase> RefreshTokenAsync(RefreshTokenRequest tokenRequest)
    {
        var storedToken = await context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

        if (storedToken == null || storedToken.IsUsed || storedToken.IsRevoked)
        {
            return new AuthResultBase
            {
                Success = false,
                Errors = ["Invalid token"]
            };
        }

        storedToken.IsUsed = true;
        context.RefreshTokens.Update(storedToken);
        await context.SaveChangesAsync();

        var dbUser = await userManager.FindByIdAsync(storedToken.UserId);
        return await GenerateJwtToken(dbUser);
    }
    
    public async Task<string?> GeneratePasswordResetTokenAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return null;
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var payload = new
        {
            Email = email,
            Token = token
        };

        var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Email", payload.Email),
                new Claim("Token", payload.Token)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var jwtToken = jwtTokenHandler.CreateToken(tokenDescriptor);
        var serializedToken = jwtTokenHandler.WriteToken(jwtToken);

        // Send the serializedToken via email to the user
        // For example, using an email service

        var resetUrl = $"http://localhost:5173/reset-password?token={serializedToken}";

        return resetUrl;
    }

    public async Task<AuthResultBase> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
    {   
        var (email, token) = DecodeJwt(resetPasswordRequest.Token);
        
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return new AuthResultBase
            {
                Success = false,
                Errors = new List<string> { "User not found" }
            };
        }
        
        // Check if the new password is one of the old passwords
        var oldPasswords = await context.OldPasswords
            .Where(op => op.UserId == user.Id)
            .Select(op => op.PasswordHash)
            .ToListAsync();

        if (oldPasswords.Any(oldPassword => userManager.PasswordHasher.VerifyHashedPassword(user, oldPassword, resetPasswordRequest.NewPassword) == PasswordVerificationResult.Success))
        {
            return new AuthResultBase
            {
                Success = false,
                Errors = ["New password cannot be one of the old passwords"]
            };
        }

        var resetPassResult = await userManager.ResetPasswordAsync(user, token, resetPasswordRequest.NewPassword);
        if (!resetPassResult.Succeeded)
        {
            return new AuthResultBase
            {
                Success = false,
                Errors = resetPassResult.Errors.Select(e => e.Description).ToList()
            };
        }
        
        var oldPasswordEntity = new OldPasswordEntity
        {
            UserId = user.Id,
            PasswordHash = userManager.PasswordHasher.HashPassword(user, resetPasswordRequest.NewPassword),
            CreatedAt = DateTime.UtcNow
        };
        
        context.OldPasswords.Add(oldPasswordEntity);
        await context.SaveChangesAsync();
            
        return new AuthResultBase
        {
            Success = true
        };
    }

    public async Task<bool> ValidateEmailAsync(string email)
    {
        return await context.Users.AnyAsync(x => x.Email == email);
    }
    
    private static (string Email, string Token) DecodeJwt(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtTokenObj = tokenHandler.ReadJwtToken(jwtToken);

        var email = jwtTokenObj.Claims.First(claim => claim.Type == "Email").Value;
        var token = jwtTokenObj.Claims.First(claim => claim.Type == "Token").Value;

        return (email, token);
    }

    private async Task<AuthResultBase> GenerateJwtToken(UserEntity user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

        var roles = await userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }.Concat(roleClaims)),
            Expires = DateTime.UtcNow.AddMinutes(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);

        var refreshToken = new RefreshTokenEntity
        {
            JwtId = token.Id,
            IsUsed = false,
            IsRevoked = false,
            UserId = user.Id,
            AddedDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddDays(6),
            Token = RandomString(35) + Guid.NewGuid(),
        };

        await context.RefreshTokens.AddAsync(refreshToken);
        await context.SaveChangesAsync();

        return new AuthResultBase
        {
            Token = jwtToken,
            Success = true,
            RefreshToken = refreshToken.Token,
        };
    }

    private static string RandomString(int length)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(x => x[random.Next(x.Length)]).ToArray());
    }
    }