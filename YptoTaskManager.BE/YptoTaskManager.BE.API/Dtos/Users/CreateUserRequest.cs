namespace YptoTaskManager.BE.API.Dtos.Users;

public record CreateUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string PasswordHash);