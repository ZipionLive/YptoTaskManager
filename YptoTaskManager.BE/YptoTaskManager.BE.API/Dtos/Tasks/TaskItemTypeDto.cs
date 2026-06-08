namespace YptoTaskManager.BE.API.Dtos.Tasks;

public record TaskItemTypeDto(
    int Id,
    string Name,
    string Description,
    int? ParentId);
