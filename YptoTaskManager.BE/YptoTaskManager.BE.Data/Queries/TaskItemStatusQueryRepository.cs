using Microsoft.EntityFrameworkCore;
using YptoTaskManager.BE.Domain.Entities;
using YptoTaskManager.BE.IData.Queries;

namespace YptoTaskManager.BE.Data.Queries;

public class TaskItemStatusQueryRepository : ITaskItemStatusQueryRepository
{
    private readonly YptoTaskManagerDbContext _context;

    public TaskItemStatusQueryRepository(YptoTaskManagerDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<TaskItemStatus>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TaskItemStatuses
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public Task<TaskItemStatus?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.TaskItemStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}