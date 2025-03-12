using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RoofEstimation.BLL.Mappers.Auth;
using RoofEstimation.DAL;
using RoofEstimation.Entities.Auth;
using RoofEstimation.Entities.Enums;
using RoofEstimation.Models.Auth;
using RoofEstimation.Models.Auth.Requests;
using RoofEstimation.Models.Configs;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace RoofEstimation.BLL.Services.Auth;

public class AuthService : IAuthService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtConfig _jwtConfig;
        private readonly ApplicationDbContext _context;

        public AuthService(
            UserManager<UserEntity> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _context = context;
        }

        public async Task<UserReponse> GetUserAsync(ClaimsPrincipal user)
        {   
            var userId  = _userManager.GetUserId(user);
            var authUser = await _userManager.FindByEmailAsync(userId);
            var role = await _userManager.GetRolesAsync(authUser);

            if (authUser == null)
            {
                throw new Exception();
            }

            return new UserReponse
            {
                Email = authUser.Email,
                FirstName = authUser.FirstName,
                LastName = authUser.LastName,
                Roles = role,
            };
        }

        public async Task<AuthResultBase> RegisterAsync(UserRegistrationRequest userRegisterRequest)
        {
            var isCompanyRoleExistsAsync = await _roleManager.RoleExistsAsync("Company");

            if (!isCompanyRoleExistsAsync)
            {
                var role = new IdentityRole { Name = "Company" };
                await _roleManager.CreateAsync(role);
            }

            var isClientRoleExist = await _roleManager.RoleExistsAsync("Client");

            if (!isClientRoleExist)
            {
                var role = new IdentityRole { Name = "Client" };
                await _roleManager.CreateAsync(role);
            }

            var userRole = userRegisterRequest.UserType == UserType.Client ? "Client" : "Company";
            var newUser = UserRegisterReqToUserEntityMapper.MapToUserEntity(userRegisterRequest);
            var isCreated = await _userManager.CreateAsync(newUser, userRegisterRequest.Password);

            if (isCreated.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, userRole);
                return await GenerateJwtToken(newUser);
            }

            return new AuthResultBase
            {
                Errors = isCreated.Errors.Select(x => x.Description).ToList(),
                Success = false,
            };
        }

        public async Task<AuthResultBase> LoginAsync(UserLoginRequest user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser == null)
            {
                return new AuthResultBase
                {
                    Errors = new List<string> { "User doesn't exist" },
                    Success = false
                };
            }

            var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

            if (!isCorrect)
            {
                return new AuthResultBase
                {
                    Errors = new List<string> { "Password is incorrect" },
                    Success = false
                };
            }

            return await GenerateJwtToken(existingUser);
        }

        public async Task<AuthResultBase> RefreshTokenAsync(RefreshTokenRequest tokenRequest)
        {
            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

            if (storedToken == null || storedToken.IsUsed || storedToken.IsRevoked)
            {
                return new AuthResultBase
                {
                    Success = false,
                    Errors = new List<string> { "Invalid token" }
                };
            }

            storedToken.IsUsed = true;
            _context.RefreshTokens.Update(storedToken);
            await _context.SaveChangesAsync();

            var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);
            return await GenerateJwtToken(dbUser);
        }

        public async Task<bool> ValidateEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        private async Task<AuthResultBase> GenerateJwtToken(UserEntity user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
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

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResultBase
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token,
            };
        }

        private string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)]).ToArray());
        }
    }