using YptoTaskManager.BE.Domain.Entities;

namespace YptoTaskManager.BE.IData.Commands;

public interface IUserCommandRepository
{
    Task<User?> GetForUpdateAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    void Update(User user);
    void Delete(User user);
}