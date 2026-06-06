using Microsoft.EntityFrameworkCore;
using YptoTaskManager.BE.Domain.Entities;
using YptoTaskManager.BE.IData.Queries;

namespace YptoTaskManager.BE.Data.Queries;

public class TaskItemTypeQueryRepository : ITaskItemTypeQueryRepository
{
    private readonly YptoTaskManagerDbContext _context;

    public TaskItemTypeQueryRepository(YptoTaskManagerDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<TaskItemType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TaskItemTypes
            .AsNoTracking()
            .OrderBy(x => x.ParentId)
            .ThenBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TaskItemType>> GetRootTypesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TaskItemTypes
            .AsNoTracking()
            .Where(x => x.ParentId == null)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TaskItemType>> GetChildrenAsync(int parentId, CancellationToken cancellationToken = default)
    {
        return await _context.TaskItemTypes
            .AsNoTracking()
            .Where(x => x.ParentId == parentId)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<TaskItemType?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.TaskItemTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}