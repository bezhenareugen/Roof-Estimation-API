using System.ComponentModel.DataAnnotations;

namespace RoofEstimation.Models.Auth.Requests;

public class ValidateEmailRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}