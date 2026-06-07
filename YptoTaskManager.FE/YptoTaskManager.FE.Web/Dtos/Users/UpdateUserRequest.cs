namespace YptoTaskManager.FE.Web.Dtos.Users;

public record UpdateUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber);