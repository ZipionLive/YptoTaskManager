using YptoTaskManager.FE.Web.Dtos.Auth;

namespace YptoTaskManager.FE.Web.Services.Auth;

public interface IAuthApiClient
{
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
