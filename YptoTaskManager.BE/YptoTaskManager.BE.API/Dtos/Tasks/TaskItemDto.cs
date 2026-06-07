namespace YptoTaskManager.BE.API.Dtos.Tasks;

public record TaskItemDto(
    int Id,
    string Name,
    int TaskTypeId,
    string TaskTypeName,
    int TaskStatusId,
    string TaskStatusName,
    Guid CreatedById,
    DateTime CreatedOn,
    IReadOnlyCollection<Guid> AssignedUserIds);