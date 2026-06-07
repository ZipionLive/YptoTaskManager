using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using YptoTaskManager.FE.Web.Dtos.Tasks;
using YptoTaskManager.FE.Web.Extensions;

namespace YptoTaskManager.FE.Web.Services.Tasks;

public class TaskItemsApiClient : ITaskItemsApiClient
{
    private readonly HttpClient _httpClient;

    public TaskItemsApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<TaskItemDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<IReadOnlyCollection<TaskItemDto>>( "api/tasks", cancellationToken) ?? [];
    }

    public async Task<IReadOnlyCollection<TaskItemDto>> SearchAsync(
        string? name,
        int? typeId,
        int? statusId,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();

        if (!string.IsNullOrWhiteSpace(name))
            query["name"] = name;

        if (typeId.HasValue)
            query["typeId"] = typeId.Value.ToString();

        if (statusId.HasValue)
            query["statusId"] = statusId.Value.ToString();

        var url = QueryHelpers.AddQueryString("api/tasks/search", query);

        return await _httpClient.GetFromJsonAsync<IReadOnlyCollection<TaskItemDto>>(url, cancellationToken) ?? [];
    }

    public async Task<TaskItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"api/tasks/{id}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound) { return null; }

        await response.EnsureSuccessAsync(cancellationToken);

        return await response.Content.ReadFromJsonAsync<TaskItemDto>(cancellationToken);
    }

    public async Task<TaskItemDto> CreateAsync(CreateTaskItemRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/tasks", request, cancellationToken);

        await response.EnsureSuccessAsync(cancellationToken);

        return await response.Content.ReadFromJsonAsync<TaskItemDto>(cancellationToken)
            ?? throw new ApiException((int)response.StatusCode, "The API returned an empty task response.");
    }

    public async Task UpdateAsync(int id, UpdateTaskItemRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/tasks/{id}", request, cancellationToken);

        await response.EnsureSuccessAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, Guid deletedById, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"/api/tasks/{id}?deleteById={deletedById}", cancellationToken);

        await response.EnsureSuccessAsync(cancellationToken);
    }
}
