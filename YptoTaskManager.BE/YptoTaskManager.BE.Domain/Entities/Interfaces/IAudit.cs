namespace YptoTaskManager.BE.Domain.Entities.Interfaces
{
    public interface IAudit
    {
        Guid CreatedById { get; set; }
        User CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }
        Guid? ModifiedById { get; set; }
        User? ModifiedBy { get; set; }
        DateTime? ModifiedOn { get; set; }
    }
}
