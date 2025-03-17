using Microsoft.AspNetCore.Identity;
using RoofEstimation.Entities.Enums;
using RoofEstimation.Entities.Enums.Auth;
using RoofEstimation.Entities.PipeInfo;
using RoofEstimation.Entities.RoofInfo;

namespace RoofEstimation.Entities.Auth;

public class UserEntity : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int Zip { get; set; }
    public List<AdditionalAddressEntity>? AdditionalAddresses { get; set; } = [];
    public string? CompanyName { get; set; }
    public string? LicenseNumber { get; set; }
    public UserType UserType { get; set; }
    public CompanyType? CompanyType { get; set; }
    
    public RoofInfoEntity? RoofInfo { get; set; }
    public PipeInfoEntity? PipeInfo { get; set; }
    public bool Gutters { get; set; } = true;
    public bool PermitFees { get; set; } = true;
    
}