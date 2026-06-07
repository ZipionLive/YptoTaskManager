using YptoTaskManager.BE.Domain.Entities;

namespace YptoTaskManager.BE.IData.Commands;

public interface ITaskItemCommandRepository
{
    Task<TaskItem?> GetForUpdateAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
    void Update(TaskItem taskItem);
    Task UpdateAssignmentsAsync(
        int taskId,
        IEnumerable<Guid> assignedUserIds,
        CancellationToken cancellationToken = default);
}