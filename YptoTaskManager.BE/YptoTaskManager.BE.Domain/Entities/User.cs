using YptoTaskManager.BE.Domain.Entities.Interfaces;
using YptoTaskManager.BE.Domain.Enums;

namespace YptoTaskManager.BE.Domain.Entities;

public class User : IEntity<Guid>
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public Guid PasswordSalt { get; set; } = Guid.NewGuid();
    public required string PasswordHash { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
    public ICollection<TaskItem> CreatedTasks { get; set; } = [];
    public ICollection<TaskItem> AssignedTasks { get; set; } = [];
}
