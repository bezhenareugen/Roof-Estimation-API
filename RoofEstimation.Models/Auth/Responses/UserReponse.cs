namespace RoofEstimation.Models.Auth;

public class UserReponse
{
    public string FullName { get; set; }
    public string Email { get; set; }

    public IList<string> Roles { get; set; }
}