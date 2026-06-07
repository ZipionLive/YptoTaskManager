using System.Net;
using YptoTaskManager.FE.Web.Dtos.Users;
using YptoTaskManager.FE.Web.Extensions;

namespace YptoTaskManager.FE.Web.Services.Users;

public class UsersApiClient : IUsersApiClient
{
    private readonly HttpClient _httpClient;

    public UsersApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<IReadOnlyCollection<UserDto>>("api/users", cancellationToken) ?? [];
    }

    public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"api/users/{id}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound) { return null; }

        await response.EnsureSuccessAsync(cancellationToken);

        return await response.Content.ReadFromJsonAsync<UserDto>(cancellationToken);
    }

    public async Task<UserDto> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/users", request, cancellationToken);

        await response.EnsureSuccessAsync(cancellationToken);

        return await response.Content.ReadFromJsonAsync<UserDto>(cancellationToken)
            ?? throw new ApiException((int)response.StatusCode, "The API returned an empty user response.");
    }

    public async Task UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/users/{id}", request, cancellationToken);

        await response.EnsureSuccessAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"api/users/{id}", cancellationToken);

        await response.EnsureSuccessAsync(cancellationToken);
    }
}
