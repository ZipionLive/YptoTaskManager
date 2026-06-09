using YptoTaskManager.BE.Domain.Entities.Interfaces;

namespace YptoTaskManager.BE.Domain.Entities;

public class TaskItemStatus : IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
