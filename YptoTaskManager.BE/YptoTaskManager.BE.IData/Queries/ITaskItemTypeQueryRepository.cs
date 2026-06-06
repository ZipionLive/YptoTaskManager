using YptoTaskManager.BE.Domain.Entities;

namespace YptoTaskManager.BE.IData.Queries;

public interface ITaskItemTypeQueryRepository
{
    Task<IReadOnlyCollection<TaskItemType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TaskItemType>> GetRootTypesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TaskItemType>> GetChildrenAsync(int parentId, CancellationToken cancellationToken = default);
    Task<TaskItemType?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}