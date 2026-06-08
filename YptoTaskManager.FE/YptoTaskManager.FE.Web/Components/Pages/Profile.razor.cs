using Microsoft.AspNetCore.Components;
using YptoTaskManager.FE.Web.Dtos.Users;
using YptoTaskManager.FE.Web.Services;
using YptoTaskManager.FE.Web.State.ActiveUser;

namespace YptoTaskManager.FE.Web.Components.Pages;

public partial class Profile
{
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private string _email = string.Empty;
    private string _phoneNumber = string.Empty;

    private bool _isSaving;
    private string? _errorMessage;
    private string? _successMessage;

    protected override void OnInitialized()
    {
        var user = ActiveUserState.Value.User;

        if (user is null)
        {
            return;
        }

        _firstName = user.FirstName;
        _lastName = user.LastName;
        _email = user.Email;
        _phoneNumber = user.PhoneNumber;
    }

    private async Task SaveAsync()
    {
        var activeUser = ActiveUserState.Value.User;

        if (activeUser is null)
        {
            return;
        }

        _errorMessage = null;
        _successMessage = null;
        _isSaving = true;

        try
        {
            var request = new UpdateUserRequest(
                _firstName,
                _lastName,
                _email,
                _phoneNumber);

            await UsersApiClient.UpdateAsync(
                activeUser.Id,
                request);

            var updatedUser =
                await UsersApiClient.GetByIdAsync(activeUser.Id);

            if (updatedUser is not null)
            {
                Dispatcher.Dispatch(new SetActiveUserAction(updatedUser));
            }

            _successMessage = "Profile updated successfully.";
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
            _isSaving = false;
        }
    }

    private void GoToLogin()
    {
        NavigationManager.NavigateTo("/login");
    }
}