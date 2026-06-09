using YptoTaskManager.FE.Web.Dtos.Auth;
using YptoTaskManager.FE.Web.Extensions;

namespace YptoTaskManager.FE.Web.Services.Auth;

public class AuthApiClient : IAuthApiClient
{
    private readonly HttpClient _httpClient;

    public AuthApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", request, cancellationToken);

        await response.EnsureSuccessAsync(cancellationToken);

        return await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken)
            ?? throw new ApiException((int)response.StatusCode, "The API returned an empty login response");
    }
}
