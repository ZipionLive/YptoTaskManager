using YptoTaskManager.BE.Domain.Entities;
using YptoTaskManager.BE.IData.Commands;

namespace YptoTaskManager.BE.Data.Commands;

public class UserCommandRepository : IUserCommandRepository
{
    private readonly YptoTaskManagerDbContext _context;

    public UserCommandRepository(YptoTaskManagerDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        return _context.Users.AddAsync(user, cancellationToken).AsTask();
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    public void Delete(User user)
    {
        _context.Users.Remove(user);
    }
}