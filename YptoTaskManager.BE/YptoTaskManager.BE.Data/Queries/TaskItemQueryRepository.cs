using Microsoft.EntityFrameworkCore;
using YptoTaskManager.BE.Domain.Entities;
using YptoTaskManager.BE.IData.Queries;

namespace YptoTaskManager.BE.Data.Queries;

public class TaskItemQueryRepository : ITaskItemQueryRepository
{
    private readonly YptoTaskManagerDbContext _context;

    public TaskItemQueryRepository(YptoTaskManagerDbContext context)
    {
        _context = context;
    }

    public Task<TaskItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.TaskItems
            .AsNoTracking()
            .Include(x => x.CreatedBy)
            .Include(x => x.TaskType)
            .Include(x => x.TaskStatus)
            .Include(x => x.AssignedTo)
            .FirstOrDefaultAsync(x => x.Id == id && x.DeletedOn == null, cancellationToken);
    }

    public async Task<IReadOnlyCollection<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TaskItems
            .AsNoTracking()
            .Include(x => x.TaskType)
            .Include(x => x.TaskStatus)
            .Include(x => x.AssignedTo)
            .Where(x => x.DeletedOn == null)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TaskItem>> GetByCreatorAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.TaskItems
            .AsNoTracking()
            .Include(x => x.TaskType)
            .Include(x => x.TaskStatus)
            .Where(x => x.CreatedById == userId && x.DeletedOn == null)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TaskItem>> GetByAssigneeAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.TaskItems
            .AsNoTracking()
            .Include(x => x.TaskType)
            .Include(x => x.TaskStatus)
            .Include(x => x.AssignedTo)
            .Where(x => x.DeletedOn == null && x.AssignedTo.Any(u => u.Id == userId))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TaskItem>> SearchAsync(string? name, int? typeId, int? statusId, CancellationToken cancellationToken = default)
    {
        IQueryable<TaskItem> query = _context.TaskItems
            .AsNoTracking()
            .Include(x => x.TaskType)
            .Include(x => x.TaskStatus)
            .Include(x => x.AssignedTo)
            .Where(x => x.DeletedOn == null);

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(x => x.Name.Contains(name));

        if (typeId.HasValue)
            query = query.Where(x => x.TaskTypeId == typeId.Value);

        if (statusId.HasValue)
            query = query.Where(x => x.TaskStatusId == statusId.Value);

        return await query.ToListAsync(cancellationToken);
    }
}