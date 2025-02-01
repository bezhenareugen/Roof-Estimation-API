using Microsoft.AspNetCore.Identity;
using RoofEstimation.Entities.PipeInfo;
using RoofEstimation.Entities.RoofInfo;

namespace RoofEstimation.Entities.Auth;

public class UserEntity : IdentityUser
{
    public RoofInfoEntity RoofInfo { get; set; }
    public PipeInfoEntity PipeInfo { get; set; }
    public bool Gutters { get; set; } = true;
    public bool PermitFees { get; set; } = true;
}