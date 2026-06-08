using YptoTaskManager.BE.API.Dtos.Tasks;
using YptoTaskManager.BE.Domain.Entities;

namespace YptoTaskManager.BE.API.Extensions;

public static class TaskItemExtensions
{
    public static TaskItemDto ToDto(this TaskItem task)
    {
        return new TaskItemDto(
            task.Id,
            task.Name,
            task.TaskTypeId,
            task.TaskType?.Name ?? string.Empty,
            task.TaskStatusId,
            task.TaskStatus?.Name ?? string.Empty,
            task.CreatedById,
            task.CreatedOn,
            task.AssignedTo.Select(x => x.Id).ToList());
    }

    public static TaskItemTypeDto ToDto(this TaskItemType taskType)
    {
        return new TaskItemTypeDto(
            taskType.Id,
            taskType.Name,
            taskType.Description,
            taskType.ParentId);
    }
}
