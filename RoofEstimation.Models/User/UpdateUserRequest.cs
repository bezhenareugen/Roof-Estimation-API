using System.ComponentModel.DataAnnotations;
using RoofEstimation.Entities.Enums.Auth;

namespace RoofEstimation.Models.User;

public class UpdateUserRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int Zip { get; set; }
    public string? CompanyName { get; set; }
    public string? LicenseNumber { get; set; }
    public CompanyType? CompanyType { get; set; }
}