using YptoTaskManager.BE.Domain.Entities;

namespace YptoTaskManager.BE.IBusiness;

public interface ITaskItemService
{
    Task<TaskItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TaskItem>> SearchAsync(
        string? name,
        int? typeId,
        int? statusId,
        CancellationToken cancellationToken = default);
    Task<TaskItem> CreateAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
    Task UpdateAsync(TaskItem taskItem, IEnumerable<Guid> assignedUserIds, Guid modifyById, CancellationToken cancelToken = default);
    Task DeleteAsync(int taskId, Guid deleteById, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TaskItemType>> GetRootTypesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TaskItemType>> GetChildrenTypesAsync(int parentId, CancellationToken cancellationToken = default);
}