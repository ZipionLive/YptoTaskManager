using YptoTaskManager.BE.Domain.Entities;
using YptoTaskManager.BE.IData;
using YptoTaskManager.BE.IData.Commands;
using YptoTaskManager.BE.IData.Queries;
using YptoTaskManager.BE.IBusiness;

namespace YptoTaskManager.BE.Business;

public class UserService : IUserService
{
    private readonly IUserQueryRepository _queryRepository;
    private readonly IUserCommandRepository _commandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(
        IUserQueryRepository queryRepository,
        IUserCommandRepository commandRepository,
        IUnitOfWork unitOfWork)
    {
        _queryRepository = queryRepository;
        _commandRepository = commandRepository;
        _unitOfWork = unitOfWork;
    }

    public Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _queryRepository.GetByIdAsync(id, cancellationToken);
    }

    public Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return _queryRepository.GetByEmailAsync(email, cancellationToken);
    }

    public Task<IReadOnlyCollection<User>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _queryRepository.GetAllAsync(cancellationToken);
    }

    public async Task<User> CreateAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);

        User? existingUser =
            await _queryRepository.GetByEmailAsync(
                user.Email,
                cancellationToken);

        if (existingUser is not null)
        {
            throw new InvalidOperationException(
                $"User '{user.Email}' already exists.");
        }

        await _commandRepository.AddAsync(
            user,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return user;
    }

    public async Task UpdateAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);

        _commandRepository.Update(user);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        User? user =
            await _queryRepository.GetByIdAsync(
                id,
                cancellationToken);

        if (user is null)
        {
            throw new InvalidOperationException(
                $"User {id} not found.");
        }

        _commandRepository.Delete(user);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}