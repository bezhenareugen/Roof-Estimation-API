using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using RoofEstimation.DAL;
using RoofEstimation.Entities.Auth;
using RoofEstimation.Models.Estimation;

namespace RoofEstimation.BLL.Services.EstimationService;

public class EstimationService(ApplicationDbContext dbContext, UserManager<UserEntity> userManager) : IEstimationService
{
    public async Task<Task> GetUserEstimations(ClaimsPrincipal authUser)
    {
        var userId  = userManager.GetUserId(authUser);
        var user = await userManager.FindByEmailAsync(userId);

        if (user == null)
        {
            throw new Exception("User not found for estimation...");
        }

        return Task.CompletedTask;
    }
}