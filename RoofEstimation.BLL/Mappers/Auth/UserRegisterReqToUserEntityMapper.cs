using RoofEstimation.Entities.Auth;
using RoofEstimation.Models.Auth.Requests;

namespace RoofEstimation.BLL.Mappers.Auth;

public static class UserRegisterReqToUserEntityMapper
{
    public static UserEntity MapToUserEntity(UserRegistrationRequest request)
    {
        return new UserEntity
        {
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.Phone,
            Address = request.Address,
            City = request.City,
            State = request.State,
            Zip = request.Zip,
            CompanyName = request.CompanyName,
            LicenseNumber = request.LicenseNumber,
            UserType = request.UserType,
            CompanyType = request.CompanyType
        };
    }
}