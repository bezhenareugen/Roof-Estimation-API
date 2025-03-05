using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RoofEstimation.DAL;
using RoofEstimation.Entities.Auth;
using RoofEstimation.Models.Auth;
using RoofEstimation.Models.Auth.Requests;
using RoofEstimation.Models.Configs;

namespace RoofEstimation.Api.Controllers.Auth;

[ApiController]
    [Route("api/[controller]")]
    public class AuthController(
        UserManager<UserEntity> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptionsMonitor<JwtConfig> optionsMonitor,
        TokenValidationParameters tokenValidationParameters,
        ApplicationDbContext context)
        : ControllerBase
    {
        private readonly JwtConfig _jwtConfig = optionsMonitor.CurrentValue;

        [HttpGet]
        [Route("User")]
        [Authorize]
        public async Task<UserReponse> GetUser()
        {
            var userId = userManager.GetUserId(User);

            var authUser = await userManager.FindByEmailAsync(userId);

            var role = await userManager.GetRolesAsync(authUser);
            
            if (authUser == null)
            {
                throw new Exception();
            }

            var user = new UserReponse()
            {
                Email = authUser.Email,
                FullName = authUser.UserName,
                Roles = role,
            };
            return user;
        } 
        
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest user)
        {
            if(ModelState.IsValid)
            {
                var isAdminRoleExist = await roleManager.RoleExistsAsync("Admin");

                if (!isAdminRoleExist)
                {
                    var role = new IdentityRole
                    {
                        Name = "Admin",
                    };
                    
                    await roleManager.CreateAsync(role);
                }
                
                // We can utilise the model
                var existingUser = await userManager.FindByEmailAsync(user.Email);

                if(existingUser != null)
                {
                    return BadRequest(new UserRegistrationResponse(){
                            Errors = new List<string>() {
                                "Email already in use"
                            },
                            Success = false
                    });
                }

                var newUser = new UserEntity() { Email = user.Email, UserName = user.UserName};
                var isCreated = await userManager.CreateAsync(newUser, user.Password);
                if(isCreated.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser ,"Admin");
                    var jwtToken = await GenerateJwtToken( newUser);

                   return Ok(jwtToken);
                } else {
                    return BadRequest(new UserRegistrationResponse(){
                            Errors = isCreated.Errors.Select(x => x.Description).ToList(),
                            Success = false,
                    });
                }
            }

            return BadRequest(new UserRegistrationResponse(){
                    Errors = new List<string>() {
                        "Invalid payload"
                    },
                    Success = false
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if(ModelState.IsValid)
            {
                var existingUser = await userManager.FindByEmailAsync(user.Email);
        
                if(existingUser == null) {
                        return BadRequest(new UserRegistrationResponse(){
                            Errors = new List<string>() {
                                "UserReponse doesn't exist"
                            },
                            Success = false
                    });
                }
        
                var isCorrect = await userManager.CheckPasswordAsync(existingUser, user.Password);
                
                if(!isCorrect) {
                      return BadRequest(new UserRegistrationResponse(){
                            Errors = new List<string>() {
                                "Password is incorrect"
                            },
                            Success = false
                    });
                }
        
                var jwtToken= await GenerateJwtToken(existingUser);
        
                return Ok(jwtToken);
            }
        
            return BadRequest(new UserRegistrationResponse(){
                    Errors = new List<string>() {
                        "Invalid payload"
                    },
                    Success = false
            });
        }

        private async Task<AuthResultBase> GenerateJwtToken(UserEntity userResponse)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new []
                {
                    new Claim("Id", userResponse.Id), 
                    new Claim(JwtRegisteredClaimNames.Email, userResponse.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, userResponse.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshTokenEntity()
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevoked = false,
                UserId = userResponse.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(6),
                Token = RandomString(35) + Guid.NewGuid(),
            };

            await context.RefreshTokens.AddAsync(refreshToken);

            await context.SaveChangesAsync();

            return new AuthResultBase()
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token,
            };
        }
        
        
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest tokenRequest)
        {
            if(ModelState.IsValid)
            {
                var result = await VerifyAndGenerateToken(tokenRequest);

                if(result == null) {
                    return BadRequest(new UserRegistrationResponse() {
                        Errors = new List<string>() {
                            "Invalid tokens"
                        },
                        Success = false
                    });
                }

                return Ok(result);
            }

            return BadRequest(new UserRegistrationResponse() {
                Errors = new List<string>() {
                    "Invalid payload"
                },
                Success = false
            });
        }

        private async Task<AuthResultBase> VerifyAndGenerateToken(RefreshTokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {   
                // // Validation 1 - Validation JWT token format
                // var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, tokenValidationParameters, out var validatedToken);
                //
                // // Validation 2 - Validate encryption alg
                // if(validatedToken is JwtSecurityToken jwtSecurityToken)
                // {
                //     var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                //
                //     if(result == false) {
                //         return null;
                //     }
                // }

                // // Validation 3 - validate expiry date
                // var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                //
                // var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

                // if(expiryDate > DateTime.UtcNow) {
                //     return new AuthResultBase() {
                //         Success = false,
                //         Errors = new List<string>() {
                //             "Token has not yet expired"
                //         }
                //     };
                // }

                // validation 4 - validate existence of the token
                var storedToken = await context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

                if(storedToken == null)
                {
                    return new AuthResultBase() {
                        Success = false,
                        Errors = new List<string>() {
                            "Token does not exist"
                        }
                    };
                }

                // Validation 5 - validate if used
                if(storedToken.IsUsed)
                {
                    return new AuthResultBase() {
                        Success = false,
                        Errors = new List<string>() {
                            "Token has been used"
                        }
                    };
                }

                // Validation 6 - validate if revoked
                if(storedToken.IsRevoked)
                {
                    return new AuthResultBase() {
                        Success = false,
                        Errors = new List<string>() {
                            "Token has been revoked"
                        }
                    };
                }

                // // Validation 7 - validate the id
                // var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                //
                // if(storedToken.JwtId != jti)
                // {
                //     return new AuthResultBase() {
                //         Success = false,
                //         Errors = new List<string>() {
                //             "Token doesn't match"
                //         }
                //     };
                // }

                // update current token 

                storedToken.IsUsed = true;
                context.RefreshTokens.Update(storedToken);
                await context.SaveChangesAsync();
                
                // Generate a new token
                var dbUser = await userManager.FindByIdAsync(storedToken.UserId);
                return await GenerateJwtToken(dbUser);
            }
            catch(Exception ex)
            {
                if(ex.Message.Contains("Lifetime validation failed. The token is expired.")) {

                      return new AuthResultBase() {
                        Success = false,
                        Errors = new List<string>() {
                            "Token has expired please re-login"
                        }
                    };
                
                } else {
                      return new AuthResultBase() {
                        Success = false,
                        Errors = new List<string>() {
                            "Something went wrong."
                        }
                    };
                }
            }    
        }

        [HttpPost("validateEmail")]
        public async Task<IActionResult> ValidateEmail([FromBody] ValidateEmailRequest request)
        {
            var emailExist = await context.Users.AnyAsync(x => x.Email == request.Email);
            
            return Ok(emailExist);
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1,1,0,0,0,0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTimeVal;
        }

        private string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)]).ToArray());
        }
    }
