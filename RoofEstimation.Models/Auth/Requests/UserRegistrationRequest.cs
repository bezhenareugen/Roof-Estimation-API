using System.ComponentModel.DataAnnotations;

namespace RoofEstimation.Models.Auth.Requests;

public class UserRegistrationRequest
{
    [Required]
    public string UserName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}