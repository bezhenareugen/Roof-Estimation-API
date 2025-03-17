namespace RoofEstimation.Entities.Auth;

public class AdditionalAddressEntity
{
    public int Id { get; set; }
    public bool IsActiveAddress { get; set; } // set to false when estimated
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int Zip { get; set; }
}