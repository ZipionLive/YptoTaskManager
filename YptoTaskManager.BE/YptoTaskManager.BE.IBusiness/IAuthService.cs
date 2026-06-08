using YptoTaskManager.BE.IBusiness.Dtos;

namespace YptoTaskManager.BE.IBusiness;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default);
}