using YptoTaskManager.BE.Domain.Entities;

namespace YptoTaskManager.BE.IData.Queries;

public interface IUserQueryRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<User>> GetAllAsync(CancellationToken cancellationToken = default);
}