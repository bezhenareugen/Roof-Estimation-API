namespace RoofEstimation.Models.Auth;

public class UserRegistrationResponse : AuthResultBase
{
    public string UserName { get; set; }
    public string Email { get; set; }
}