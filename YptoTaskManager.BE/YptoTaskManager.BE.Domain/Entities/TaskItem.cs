using YptoTaskManager.BE.Domain.Entities.Interfaces;

namespace YptoTaskManager.BE.Domain.Entities
{
    public class TaskItem : IEntity<int>, IAudit, ISoftDelete
    {
        public int Id { get; set; }
        public Guid CreatedById { get; set; }
        public required User CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ModifiedById { get; set; }
        public User? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? DeletedById { get; set; }
        public User? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public required string Name { get; set; }
        public int TaskTypeId { get; set; }
        public required TaskItemType TaskType { get; set; }
        public int TaskStatusId { get; set; }
        public required TaskItemStatus TaskStatus { get; set; }
        public ICollection<User> AssignedTo { get; set; } = [];
    }
}
