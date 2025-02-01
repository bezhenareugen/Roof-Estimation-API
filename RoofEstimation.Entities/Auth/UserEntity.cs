using Microsoft.AspNetCore.Identity;
using RoofEstimation.Entities.PipeInfo;
using RoofEstimation.Entities.RoofInfo;

namespace RoofEstimation.Entities.Auth;

public class UserEntity : IdentityUser
{
    List<RoofInfoEntity>? RoofInfos { get; set; }
    List<PipeInfoEntity>? PipeInfos { get; set; }
}