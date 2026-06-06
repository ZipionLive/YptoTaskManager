using YptoTaskManager.BE.Domain.Entities;

namespace YptoTaskManager.BE.IData.Commands;

public interface ITaskItemCommandRepository
{
    Task AddAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
    void Update(TaskItem taskItem);
    void Delete(TaskItem taskItem);
}