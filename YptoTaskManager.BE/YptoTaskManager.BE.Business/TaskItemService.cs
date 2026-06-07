using YptoTaskManager.BE.Domain.Entities;
using YptoTaskManager.BE.IData;
using YptoTaskManager.BE.IData.Commands;
using YptoTaskManager.BE.IData.Queries;
using YptoTaskManager.BE.IBusiness;

namespace YptoTaskManager.BE.Business;

public class TaskItemService : ITaskItemService
{
    private readonly ITaskItemQueryRepository _queryRepository;
    private readonly ITaskItemCommandRepository _commandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TaskItemService(
        ITaskItemQueryRepository queryRepository,
        ITaskItemCommandRepository commandRepository,
        IUnitOfWork unitOfWork)
    {
        _queryRepository = queryRepository;
        _commandRepository = commandRepository;
        _unitOfWork = unitOfWork;
    }

    public Task<TaskItem?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return _queryRepository.GetByIdAsync(id, cancellationToken);
    }

    public Task<IReadOnlyCollection<TaskItem>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _queryRepository.GetAllAsync(cancellationToken);
    }

    public Task<IReadOnlyCollection<TaskItem>> SearchAsync(
        string? name,
        int? typeId,
        int? statusId,
        CancellationToken cancellationToken = default)
    {
        return _queryRepository.SearchAsync(
            name,
            typeId,
            statusId,
            cancellationToken);
    }

    public async Task<TaskItem> CreateAsync(
        TaskItem taskItem,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(taskItem);

        await _commandRepository.AddAsync(
            taskItem,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return taskItem;
    }

    public async Task UpdateAsync(
    TaskItem taskItem,
    IEnumerable<Guid> assignedUserIds,
    Guid modifyById,
    CancellationToken cancellationToken = default)
    {
        TaskItem? existingTask =
            await _commandRepository.GetForUpdateAsync(
                taskItem.Id,
                cancellationToken);

        if (existingTask is null)
        {
            throw new InvalidOperationException(
                $"Task {taskItem.Id} not found.");
        }

        existingTask.Name = taskItem.Name;
        existingTask.TaskTypeId = taskItem.TaskTypeId;
        existingTask.TaskStatusId = taskItem.TaskStatusId;
        existingTask.ModifiedById = modifyById;
        existingTask.ModifiedOn = DateTime.UtcNow;

        await _commandRepository.UpdateAssignmentsAsync(
            taskItem.Id,
            assignedUserIds,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }

    public async Task DeleteAsync(
        int taskId,
        Guid deletedById,
        CancellationToken cancellationToken = default)
    {
        TaskItem? task = await _queryRepository.GetByIdAsync(
            taskId,
            cancellationToken);

        if (task is null)
        {
            throw new InvalidOperationException(
                $"Task {taskId} not found.");
        }

        task.DeletedById = deletedById;
        task.DeletedOn = DateTime.UtcNow;

        _commandRepository.Update(task);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}