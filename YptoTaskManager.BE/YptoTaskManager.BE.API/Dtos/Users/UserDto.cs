using YptoTaskManager.BE.Domain.Enums;

namespace YptoTaskManager.BE.API.Dtos.Users;

public record UserDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    UserRole Role);