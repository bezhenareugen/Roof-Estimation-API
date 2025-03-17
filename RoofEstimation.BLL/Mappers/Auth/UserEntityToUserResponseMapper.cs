using RoofEstimation.Entities.Auth;
using RoofEstimation.Models.Auth;

namespace RoofEstimation.BLL.Mappers.Auth;

public class UserEntityToUserResponseMapper
{
     public static UserResponse MapToUserResponse(UserEntity entity)
     {
         return new UserResponse
         {
             Email = entity.Email,
             FirstName = entity.FirstName,
             LastName = entity.LastName,
             Address = new UserAddress
             {
                PhoneNumber = entity.PhoneNumber,
                Address = entity.Address,
                City = entity.City,
                State = entity.State,
                Zip = entity.Zip,
             }
         };
     }
}