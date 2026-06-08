namespace YptoTaskManager.FE.Web.Dtos.Tasks;

public record TaskItemTypeDto(
    int Id,
    string Name,
    string Description,
    int? ParentId);
