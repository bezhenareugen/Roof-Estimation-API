namespace RoofEstimation.Models.Auth;

public class UserReponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public IList<string> Roles { get; set; }
}