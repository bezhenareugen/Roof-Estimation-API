using System.ComponentModel.DataAnnotations;
using RoofEstimation.Entities.Enums;
using RoofEstimation.Entities.Enums.Auth;

namespace RoofEstimation.Models.Auth.Requests;

public class UserRegistrationRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one number, and one symbol.")]
    public string Password { get; set; }
    
    public string CompanyName { get; set; }

    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }

    [Required]
    public string Phone { get; set; }

    [Required]
    public string Address { get; set; }
    
    [Required]
    public string City { get; set; }

    [Required]
    public string State { get; set; }
    
    [Required]
    public int Zip { get; set; }
    
    [Required]
    public UserType UserType { get; set; } = UserType.Client;

    [Required]
    public CompanyType CompanyType { get; set; }

    public string? LicenseNumber { get; set; }
}
