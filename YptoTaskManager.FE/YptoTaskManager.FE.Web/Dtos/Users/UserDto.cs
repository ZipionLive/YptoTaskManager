using YptoTaskManager.FE.Web.Dtos.Enums;

namespace YptoTaskManager.FE.Web.Dtos.Users;

public record UserDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    UserRole Role);