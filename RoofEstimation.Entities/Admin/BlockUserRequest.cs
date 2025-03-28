using System.ComponentModel.DataAnnotations;

namespace RoofEstimation.Entities.Admin;

public class BlockUserRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}