namespace YptoTaskManager.BE.API.Dtos.Users;

public record UserDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber);