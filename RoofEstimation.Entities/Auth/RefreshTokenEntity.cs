using System.ComponentModel.DataAnnotations.Schema;

namespace RoofEstimation.Entities.Auth;

public class RefreshTokenEntity
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Token { get; set; }
    public string JwtId { get; set; }
    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime AddedDate { get; set; }
    public DateTime ExpiryDate { get; set; }
        
    [ForeignKey(nameof(UserId))]
    public UserEntity Users { get; set; }
}