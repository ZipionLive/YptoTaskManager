namespace YptoTaskManager.FE.Web.Dtos.Users;

public record CreateUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Password);