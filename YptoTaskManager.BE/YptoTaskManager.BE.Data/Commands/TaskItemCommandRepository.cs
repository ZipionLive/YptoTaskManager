using Microsoft.EntityFrameworkCore;
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

    public Task<TaskItem?> GetForUpdateAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.TaskItems
            .Include(x => x.AssignedTo)
            .FirstOrDefaultAsync(x => x.Id == id && x.DeletedOn == null, cancellationToken);
    }

    public async Task AddAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
    {
        foreach (var assignedUser in taskItem.AssignedTo)
        {
            _context.Entry(assignedUser).State = EntityState.Unchanged;
        }

        await _context.TaskItems.AddAsync(taskItem, cancellationToken);
    }

    public void Update(TaskItem taskItem)
    {
        _context.TaskItems.Update(taskItem);
    }

    public async Task UpdateAssignmentsAsync(
        int taskId,
        IEnumerable<Guid> assignedUserIds,
        CancellationToken cancellationToken = default)
    {
        TaskItem? task = await _context.TaskItems
            .Include(x => x.AssignedTo)
            .FirstOrDefaultAsync(
                x => x.Id == taskId,
                cancellationToken);

        if (task is null)
        {
            throw new InvalidOperationException(
                $"Task {taskId} not found.");
        }

        List<Guid> userIds = assignedUserIds
            .Distinct()
            .ToList();

        List<User> users = await _context.Users
            .Where(x => userIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        task.AssignedTo.Clear();

        foreach (User user in users)
        {
            task.AssignedTo.Add(user);
        }
    }
}