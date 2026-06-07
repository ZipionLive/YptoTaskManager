namespace YptoTaskManager.BE.API.Dtos.Tasks;

public record CreateTaskItemRequest(
    string Name,
    int TaskTypeId,
    int TaskStatusId,
    Guid CreatedById,
    IReadOnlyCollection<Guid> AssignedUserIds);