using YptoTaskManager.FE.Web.Dtos.Users;

namespace YptoTaskManager.FE.Web.Services.Users;

public interface IUsersApiClient
{
    Task<IReadOnlyCollection<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserDto> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
