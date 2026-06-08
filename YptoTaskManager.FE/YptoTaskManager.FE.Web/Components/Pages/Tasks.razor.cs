using Microsoft.AspNetCore.Components;
using YptoTaskManager.FE.Web.Dtos.Tasks;
using YptoTaskManager.FE.Web.Services;
using YptoTaskManager.FE.Web.Services.Tasks;

namespace YptoTaskManager.FE.Web.Components.Pages;

public partial class Tasks
{
    [Inject]
    private ITaskItemsApiClient TaskItemsApiClient { get; set; } = default!;

    private List<TaskItemDto> _tasks = [];
    private bool _isLoading;
    private string? _errorMessage;

    private TaskItemDto? _selectedTask;
    private string _editName = string.Empty;
    private int _editTypeId;
    private int _editStatusId;
    private bool _isCreateMode;

    private IEnumerable<TaskItemDto> VisibleTasks =>
        ActiveUserState.Value.User is null
            ? []
            : _tasks.Where(task =>
                task.CreatedById == ActiveUserState.Value.User.Id
                || task.AssignedUserIds.Contains(ActiveUserState.Value.User.Id));

    private IEnumerable<TaskItemDto> TodoTasks =>
        VisibleTasks.Where(task => task.TaskStatusName.Equals("To do", StringComparison.OrdinalIgnoreCase));

    private IEnumerable<TaskItemDto> InProgressTasks =>
        VisibleTasks.Where(task => task.TaskStatusName.Equals("In progress", StringComparison.OrdinalIgnoreCase));

    private IEnumerable<TaskItemDto> DoneTasks =>
        VisibleTasks.Where(task => task.TaskStatusName.Equals("Done", StringComparison.OrdinalIgnoreCase));

    protected override async Task OnInitializedAsync()
    {
        if (ActiveUserState.Value.IsLoggedIn)
        {
            await LoadTasksAsync();
        }
    }

    private async Task LoadTasksAsync()
    {
        _isLoading = true;
        _errorMessage = null;

        try
        {
            _tasks = (await TaskItemsApiClient.GetAllAsync()).ToList();
        }
        catch (ApiException ex)
        {
            _errorMessage = $"API error {ex.StatusCode}: {ex.Message}";
        }
        catch (Exception ex)
        {
            _errorMessage = $"Unexpected error: {ex.Message}";
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void OpenTask(TaskItemDto task)
    {
        _isCreateMode = false;
        _selectedTask = task;
        _editName = task.Name;
        _editTypeId = task.TaskTypeId;
        _editStatusId = task.TaskStatusId;
    }

    private void OpenCreateTaskModal()
    {
        if (ActiveUserState.Value.User is null)
        {
            return;
        }

        _isCreateMode = true;
        _selectedTask = new TaskItemDto(
            0,
            string.Empty,
            4,
            "Development",
            1,
            "To do",
            ActiveUserState.Value.User.Id,
            DateTime.UtcNow,
            [ActiveUserState.Value.User.Id]);

        _editName = string.Empty;
        _editTypeId = 4;
        _editStatusId = 1;
    }

    private void CloseModal()
    {
        _isCreateMode = false;
        _selectedTask = null;
        _editName = string.Empty;
        _editTypeId = 0;
        _editStatusId = 0;
    }

    private async Task SaveTaskAsync()
    {
        if (_selectedTask is null || ActiveUserState.Value.User is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(_editName))
        {
            _errorMessage = "Task name is required.";
            return;
        }

        if (_isCreateMode)
        {
            var createRequest = new CreateTaskItemRequest(
                _editName,
                _editTypeId,
                _editStatusId,
                ActiveUserState.Value.User.Id,
                [ActiveUserState.Value.User.Id]);

            await TaskItemsApiClient.CreateAsync(createRequest);
        }
        else
        {
            var updateRequest = new UpdateTaskItemRequest(
                _editName,
                _editTypeId,
                _editStatusId,
                ActiveUserState.Value.User.Id,
                _selectedTask.AssignedUserIds);

            await TaskItemsApiClient.UpdateAsync(
                _selectedTask.Id,
                updateRequest);
        }

        CloseModal();

        await LoadTasksAsync();
    }

    private void GoToLogin()
    {
        NavigationManager.NavigateTo("/login");
    }
}