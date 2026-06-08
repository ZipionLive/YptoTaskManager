using Microsoft.EntityFrameworkCore;
using YptoTaskManager.BE.Domain.Entities;
using YptoTaskManager.BE.IData.Queries;

namespace YptoTaskManager.BE.Data.Queries;

public class UserQueryRepository : IUserQueryRepository
{
    private readonly YptoTaskManagerDbContext _context;

    public UserQueryRepository(YptoTaskManagerDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()), cancellationToken);
    }

    public async Task<IReadOnlyCollection<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .OrderBy(x => x.LastName)
            .ThenBy(x => x.FirstName)
            .ToListAsync(cancellationToken);
    }
}