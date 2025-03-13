using System.ComponentModel.DataAnnotations;

namespace RoofEstimation.Models.Auth.Requests;

public class RequestPasswordResetRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}