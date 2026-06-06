using YptoTaskManager.BE.Domain.Entities;
using YptoTaskManager.BE.IData.Commands;

namespace YptoTaskManager.BE.Data.Commands;

public class TaskItemCommandRepository : ITaskItemCommandRepository
{
    private readonly YptoTaskManagerDbContext _context;

    public TaskItemCommandRepository(YptoTaskManagerDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
    {
        return _context.TaskItems.AddAsync(taskItem, cancellationToken).AsTask();
    }

    public void Update(TaskItem taskItem)
    {
        _context.TaskItems.Update(taskItem);
    }

    public void Delete(TaskItem taskItem)
    {
        _context.TaskItems.Remove(taskItem);
    }
}