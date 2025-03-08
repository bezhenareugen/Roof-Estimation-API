namespace RoofEstimation.Models.Auth;

public class UserRegistrationResponse : AuthResultBase
{
    public string FullName { get; set; }
    public string Email { get; set; }
}