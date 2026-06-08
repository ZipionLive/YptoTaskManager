using Microsoft.AspNetCore.Components;
using YptoTaskManager.FE.Web.Dtos.Users;
using YptoTaskManager.FE.Web.Services;
using YptoTaskManager.FE.Web.State.ActiveUser;

namespace YptoTaskManager.FE.Web.Components.Pages;

public partial class Login
{
    private List<UserDto> _users = [];
    private Guid? _selectedUserId;
    private bool _isLoading = true;
    private string? _errorMessage;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _users = (await UsersApiClient.GetAllAsync()).ToList();
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

    private Task LoginAsync()
    {
        if (!_selectedUserId.HasValue)
        {
            return Task.CompletedTask;
        }

        var selectedUser = _users.FirstOrDefault(x => x.Id == _selectedUserId.Value);

        if (selectedUser is null)
        {
            _errorMessage = "Selected user was not found.";
            return Task.CompletedTask;
        }

        Dispatcher.Dispatch(new SetActiveUserAction(selectedUser));

        NavigationManager.NavigateTo("/tasks");

        return Task.CompletedTask;
    }
}
