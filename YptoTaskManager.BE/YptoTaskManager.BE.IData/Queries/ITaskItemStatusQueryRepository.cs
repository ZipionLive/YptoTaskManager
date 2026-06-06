using YptoTaskManager.BE.Domain.Entities;

namespace YptoTaskManager.BE.IData.Queries;

public interface ITaskItemStatusQueryRepository
{
    Task<IReadOnlyCollection<TaskItemStatus>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TaskItemStatus?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}