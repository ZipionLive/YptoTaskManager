namespace YptoTaskManager.BE.Domain.Entities.Interfaces
{
    public interface ISoftDelete
    {
        Guid? DeletedById { get; set; }
        User? DeletedBy { get; set; }
        DateTime? DeletedOn { get; set; }
    }
}
