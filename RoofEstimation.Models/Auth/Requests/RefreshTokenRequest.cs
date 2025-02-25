using System.ComponentModel.DataAnnotations;

namespace RoofEstimation.Models.Auth.Requests;

public class RefreshTokenRequest
{
    public string Token { get; set; }
    [Required]
    public string RefreshToken { get; set; }
}