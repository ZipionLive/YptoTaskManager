using YptoTaskManager.FE.Web.Dtos.Tasks;

namespace YptoTaskManager.FE.Web.Services.Tasks;

public interface ITaskItemsApiClient
{
    Task<IReadOnlyCollection<TaskItemDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TaskItemDto>> SearchAsync(
        string? name,
        int? typeId,
        int? statusId,
        CancellationToken cancellationToken = default);
    Task<TaskItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TaskItemDto> CreateAsync(CreateTaskItemRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, UpdateTaskItemRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, Guid deletedById, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TaskItemTypeDto>> GetRootTypesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TaskItemTypeDto>> GetChildrenTypesAsync(int parentId, CancellationToken cancellationToken = default);

}
