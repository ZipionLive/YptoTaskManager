using YptoTaskManager.BE.API.Dtos.Users;
using YptoTaskManager.BE.Domain.Entities;

namespace YptoTaskManager.BE.API.Extensions
{
    public static class UserExtensions
    {
        public static UserDto ToDto(this User user)
        {
            return new UserDto(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.PhoneNumber);
        }
    }
}
