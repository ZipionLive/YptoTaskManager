namespace YptoTaskManager.BE.API.Dtos.Auth
{
    public record LoginResponse(
        string Token,
        Guid UserId,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber);
}
