using YptoTaskManager.BE.Domain.Entities.Interfaces;

namespace YptoTaskManager.BE.Domain.Entities
{
    public class TaskItemType : IEntity<int>, IAudit, ISoftDelete
    {
        public int Id { get; set; }
        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ModifiedById { get; set; }
        public User? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? DeletedById { get; set; }
        public User? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public TaskItemType Parent { get; set; }
    }
}
