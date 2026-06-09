using Fluxor;
using Microsoft.AspNetCore.Components;
using YptoTaskManager.FE.Web.Dtos.Enums;
using YptoTaskManager.FE.Web.Dtos.Users;
using YptoTaskManager.FE.Web.Services;
using YptoTaskManager.FE.Web.Services.Users;
using YptoTaskManager.FE.Web.State.ActiveUser;

namespace YptoTaskManager.FE.Web.Components.Pages;

public partial class Users
{
    [Inject]
    private IUsersApiClient UsersApiClient { get; set; } = default!;

    [Inject]
    private IState<ActiveUserState> ActiveUserState { get; set; } = default!;

    private List<UserDto> _users = [];
    private bool _isLoading;
    private string? _errorMessage;
    private bool _isCreateMode;
    private string _password = string.Empty;

    private UserDto? _editedUser;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private string _email = string.Empty;
    private string _phoneNumber = string.Empty;

    private bool IsAdmin =>
        ActiveUserState.Value.User?.Role == UserRole.Admin;

    protected override async Task OnInitializedAsync()
    {
        if (IsAdmin)
        {
            await LoadUsersAsync();
        }
    }

    private async Task LoadUsersAsync()
    {
        _isLoading = true;
        _errorMessage = null;

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

    private void OpenCreateModal()
    {
        _isCreateMode = true;
        _editedUser = null;

        _firstName = string.Empty;
        _lastName = string.Empty;
        _email = string.Empty;
        _phoneNumber = string.Empty;
        _password = string.Empty;
    }

    private void OpenEditModal(UserDto user)
    {
        _isCreateMode = false;
        _editedUser = user;

        _firstName = user.FirstName;
        _lastName = user.LastName;
        _email = user.Email;
        _phoneNumber = user.PhoneNumber;
        _password = string.Empty;
    }

    private void CloseModal()
    {
        _isCreateMode = false;
        _editedUser = null;

        _firstName = string.Empty;
        _lastName = string.Empty;
        _email = string.Empty;
        _phoneNumber = string.Empty;
        _password = string.Empty;
    }

    private async Task SaveUserAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_firstName) ||
                string.IsNullOrWhiteSpace(_lastName) ||
                string.IsNullOrWhiteSpace(_email) ||
                string.IsNullOrWhiteSpace(_phoneNumber))
            {
                _errorMessage = "All fields are required.";
                return;
            }

            if (_isCreateMode)
            {
                if (string.IsNullOrWhiteSpace(_password))
                {
                    _errorMessage = "Password is required.";
                    return;
                }

                var createRequest = new CreateUserRequest(
                    _firstName,
                    _lastName,
                    _email,
                    _phoneNumber,
                    _password);

                await UsersApiClient.CreateAsync(createRequest);
            }
            else
            {
                if (_editedUser is null)
                {
                    return;
                }

                var updateRequest = new UpdateUserRequest(
                    _firstName,
                    _lastName,
                    _email,
                    _phoneNumber);

                await UsersApiClient.UpdateAsync(
                    _editedUser.Id,
                    updateRequest);
            }

            CloseModal();
            await LoadUsersAsync();
        }
        catch (ApiException ex)
        {
            _errorMessage = $"API error {ex.StatusCode}: {ex.Message}";
        }
        catch (Exception ex)
        {
            _errorMessage = $"Unexpected error: {ex.Message}";
        }
    }

    private async Task DeleteUserAsync(Guid id)
    {
        if (id == ActiveUserState.Value.User?.Id)
        {
            return;
        }

        try
        {
            await UsersApiClient.DeleteAsync(id);
            await LoadUsersAsync();
        }
        catch (ApiException ex)
        {
            _errorMessage = $"API error {ex.StatusCode}: {ex.Message}";
        }
        catch (Exception ex)
        {
            _errorMessage = $"Unexpected error: {ex.Message}";
        }
    }
}