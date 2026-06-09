using Microsoft.AspNetCore.Components;
using YptoTaskManager.FE.Web.Dtos.Tasks;
using YptoTaskManager.FE.Web.Services;
using YptoTaskManager.FE.Web.Services.Tasks;
using YptoTaskManager.FE.Web.Dtos.Users;
using YptoTaskManager.FE.Web.Services.Users;

namespace YptoTaskManager.FE.Web.Components.Pages;

public partial class Tasks
{
    [Inject]
    private ITaskItemsApiClient TaskItemsApiClient { get; set; } = default!;
    [Inject]
    private IUsersApiClient UsersApiClient { get; set; } = default!;

    private List<UserDto> _users = [];
    private HashSet<Guid> _selectedAssignedUserIds = [];

    private List<TaskItemDto> _tasks = [];
    private bool _isLoading;
    private string? _errorMessage;

    private TaskItemDto? _selectedTask;
    private string _editName = string.Empty;
    private int _editTypeId;
    private int _editStatusId;
    private bool _isCreateMode;
    private List<TaskItemTypeDto> _rootTypes = [];
    private List<TaskItemTypeDto> _childrenTypes = [];
    private int? _editParentTypeId;

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
        _rootTypes = (await TaskItemsApiClient.GetRootTypesAsync()).ToList();
        _users = (await UsersApiClient.GetAllAsync()).ToList();

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

    private async Task OpenTask(TaskItemDto task)
    {
        _isCreateMode = false;
        _selectedTask = task;

        _editName = task.Name;
        _editTypeId = task.TaskTypeId;
        _editStatusId = task.TaskStatusId;

        _selectedAssignedUserIds = task.AssignedUserIds.ToHashSet();

        var childType = _rootTypes
            .SelectMany(_ => _childrenTypes)
            .FirstOrDefault(x => x.Id == task.TaskTypeId);

        // Plus fiable : retrouver le parent en testant tous les parents.
        foreach (var rootType in _rootTypes)
        {
            var children = (await TaskItemsApiClient.GetChildrenTypesAsync(rootType.Id)).ToList();

            if (children.Any(x => x.Id == task.TaskTypeId))
            {
                _editParentTypeId = rootType.Id;
                _childrenTypes = children;
                break;
            }
        }
    }

    private void OpenCreateTaskModal()
    {
        if (ActiveUserState.Value.User is null)
        {
            return;
        }

        _isCreateMode = true;
        _selectedTask = null;

        _editName = string.Empty;
        _editParentTypeId = null;
        _childrenTypes = [];
        _editTypeId = 0;
        _editStatusId = 1;

        _selectedAssignedUserIds = [ActiveUserState.Value.User.Id];
    }

    private void OnAssignedUserChanged(Guid userId, ChangeEventArgs e)
    {
        var isChecked = e.Value is bool value && value;

        if (isChecked)
        {
            _selectedAssignedUserIds.Add(userId);
        }
        else
        {
            _selectedAssignedUserIds.Remove(userId);
        }
    }

    private void CloseModal()
    {
        _isCreateMode = false;
        _selectedTask = null;
        _editName = string.Empty;
        _editTypeId = 0;
        _editStatusId = 0;
        _selectedAssignedUserIds = [];
    }

    private async Task SaveTaskAsync()
    {
        if (ActiveUserState.Value.User is null)
        {
            return;
        }

        if (!_isCreateMode && _selectedTask is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(_editName))
        {
            _errorMessage = "Task name is required.";
            return;
        }

        if (_editTypeId <= 0)
        {
            _errorMessage = "Task type is required.";
            return;
        }

        if (_isCreateMode)
        {
            var createRequest = new CreateTaskItemRequest(
                _editName,
                _editTypeId,
                _editStatusId,
                ActiveUserState.Value.User.Id,
                _selectedAssignedUserIds.ToList());

            await TaskItemsApiClient.CreateAsync(createRequest);
        }
        else
        {
            if (_selectedTask is null)
            {
                return;
            }

            var updateRequest = new UpdateTaskItemRequest(
                _editName,
                _editTypeId,
                _editStatusId,
                ActiveUserState.Value.User.Id,
                _selectedAssignedUserIds.ToList());

            await TaskItemsApiClient.UpdateAsync(
                _selectedTask.Id,
                updateRequest);
        }

        CloseModal();

        await LoadTasksAsync();
    }

    private async Task OnParentTypeChangedAsync()
    {
        _childrenTypes = [];
        _editTypeId = 0;

        if (!_editParentTypeId.HasValue)
        {
            return;
        }

        _childrenTypes = (await TaskItemsApiClient
            .GetChildrenTypesAsync(_editParentTypeId.Value))
            .ToList();
    }

    private void GoToLogin()
    {
        NavigationManager.NavigateTo("/login");
    }
}