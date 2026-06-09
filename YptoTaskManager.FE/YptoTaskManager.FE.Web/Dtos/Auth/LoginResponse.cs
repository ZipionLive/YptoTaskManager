using YptoTaskManager.FE.Web.Dtos.Enums;

namespace YptoTaskManager.FE.Web.Dtos.Auth;

public record LoginResponse(
    string Token,
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    UserRole Role);

