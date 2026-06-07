namespace YptoTaskManager.FE.Web.Dtos.Tasks;

public record UpdateTaskItemRequest(
    string Name,
    int TaskTypeId,
    int TaskStatusId,
    Guid ModifiedById,
    IReadOnlyCollection<Guid> AssignedUserIds);