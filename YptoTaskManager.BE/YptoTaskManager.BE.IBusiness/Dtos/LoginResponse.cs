namespace YptoTaskManager.BE.IBusiness.Dtos;

public record LoginResponse(
    string Token,
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber);
