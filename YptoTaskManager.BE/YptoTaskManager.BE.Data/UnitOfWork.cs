using YptoTaskManager.BE.IData;

namespace YptoTaskManager.BE.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly YptoTaskManagerDbContext _context;

    public UnitOfWork(YptoTaskManagerDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}