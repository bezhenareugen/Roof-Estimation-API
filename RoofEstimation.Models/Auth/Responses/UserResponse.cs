using RoofEstimation.Entities.Enums.Auth;

namespace RoofEstimation.Models.Auth;

public class UserResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public UserAddress Address { get; set; }
    public IList<string> Roles { get; set; }
    public string? CompanyName { get; set; }
    public CompanyType? CompanyType { get; set; }
    public string? LicenseNumber { get; set; }
}

public class UserAddress
{
    public string? PhoneNumber { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int Zip { get; set; }
}