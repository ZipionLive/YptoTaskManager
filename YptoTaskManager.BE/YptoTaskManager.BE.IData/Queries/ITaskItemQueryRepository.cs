using YptoTaskManager.BE.Domain.Entities;

namespace YptoTaskManager.BE.IData.Queries;

public interface ITaskItemQueryRepository
{
    Task<TaskItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TaskItem>> GetAllForUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TaskItem>> GetByCreatorAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TaskItem>> GetByAssigneeAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TaskItem>> SearchAsync(string? name, int? typeId, int? statusId, CancellationToken cancellationToken = default);
}