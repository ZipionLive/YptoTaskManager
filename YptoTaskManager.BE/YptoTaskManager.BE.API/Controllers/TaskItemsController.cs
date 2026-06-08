using Microsoft.AspNetCore.Mvc;
using YptoTaskManager.BE.API.Dtos.Tasks;
using YptoTaskManager.BE.API.Extensions;
using YptoTaskManager.BE.Domain.Entities;
using YptoTaskManager.BE.IBusiness;

namespace YptoTaskManager.BE.API.Controllers;

[ApiController]
[Route("api/tasks")]
[Produces("application/json")]
public class TaskItemsController : ControllerBase
{
    private readonly ITaskItemService _taskItemService;
    private readonly IUserService _userService;

    public TaskItemsController(
        ITaskItemService taskItemService,
        IUserService userService)
    {
        _taskItemService = taskItemService;
        _userService = userService;
    }

    /// <summary>
    /// Gets all non-deleted tasks.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<TaskItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<TaskItemDto>>> GetAll(CancellationToken cancellationToken)
    {
        var tasks = await _taskItemService.GetAllAsync(cancellationToken);

        return Ok(tasks.Select(t => t.ToDto()).ToList());
    }

    /// <summary>
    /// Searches tasks by name, type or status.
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IReadOnlyCollection<TaskItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<TaskItemDto>>> Search(
        [FromQuery] string? name,
        [FromQuery] int? typeId,
        [FromQuery] int? statusId,
        CancellationToken cancellationToken)
    {
        var tasks = await _taskItemService.SearchAsync(
            name,
            typeId,
            statusId,
            cancellationToken);

        return Ok(tasks.Select(t => t.ToDto()).ToList());
    }

    /// <summary>
    /// Gets a task by id.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TaskItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskItemDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var task = await _taskItemService.GetByIdAsync(id, cancellationToken);

        if (task is null)
        {
            return NotFound();
        }

        return Ok(task.ToDto());
    }

    /// <summary>
    /// Creates a new task.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TaskItemDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskItemDto>> Create(
        CreateTaskItemRequest request,
        CancellationToken cancellationToken)
    {
        var assignedUsers = new List<User>();

        foreach (var userId in request.AssignedUserIds.Distinct())
        {
            var user = await _userService.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return BadRequest($"Assigned user '{userId}' does not exist.");
            }

            assignedUsers.Add(user);
        }

        var task = new TaskItem
        {
            Name = request.Name,
            TaskTypeId = request.TaskTypeId,
            TaskStatusId = request.TaskStatusId,
            CreatedById = request.CreatedById,
            CreatedOn = DateTime.UtcNow,
            AssignedTo = assignedUsers
        };

        var createdTask = await _taskItemService.CreateAsync(task, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdTask.Id },
            createdTask.ToDto());
    }

    /// <summary>
    /// Updates an existing task.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        int id,
        UpdateTaskItemRequest request,
        CancellationToken cancellationToken)
    {
        var task = await _taskItemService.GetByIdAsync(id, cancellationToken);

        if (task is null)
        {
            return NotFound();
        }

        var assignedUsers = new List<User>();

        foreach (var userId in request.AssignedUserIds.Distinct())
        {
            var user = await _userService.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return BadRequest($"Assigned user '{userId}' does not exist.");
            }

            assignedUsers.Add(user);
        }

        task.Name = request.Name;
        task.TaskTypeId = request.TaskTypeId;
        task.TaskStatusId = request.TaskStatusId;
        task.ModifiedOn = DateTime.UtcNow;
        task.AssignedTo = assignedUsers;

        await _taskItemService.UpdateAsync(task, task.AssignedTo.Select(u => u.Id), request.ModifiedById, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Soft deletes a task.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        int id,
        [FromQuery] Guid deletedById,
        CancellationToken cancellationToken)
    {
        var task = await _taskItemService.GetByIdAsync(id, cancellationToken);

        if (task is null)
        {
            return NotFound();
        }

        await _taskItemService.DeleteAsync(id, deletedById, cancellationToken);

        return NoContent();
    }

    [HttpGet("rootTypes")]
    [ProducesResponseType(typeof(IReadOnlyCollection<TaskItemTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<TaskItemTypeDto>>> GetRootTypes(CancellationToken cancellationToken)
    {
        var taskTypes = await _taskItemService.GetRootTypesAsync(cancellationToken);

        return Ok(taskTypes.Select(tt => tt.ToDto()).ToList());
    }

    [HttpGet("childrenTypes/{parentId:int}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<TaskItemTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<TaskItemTypeDto>>> GetChildrenTypes(int parentId, CancellationToken cancellationToken)
    {
        var taskTypes = await _taskItemService.GetChildrenTypesAsync(parentId, cancellationToken);

        return Ok(taskTypes.Select(tt => tt.ToDto()).ToList());
    }
}