using Fluxor;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using YptoTaskManager.FE.Web.Dtos.Auth;
using YptoTaskManager.FE.Web.Dtos.Users;
using YptoTaskManager.FE.Web.Services;
using YptoTaskManager.FE.Web.Services.Auth;
using YptoTaskManager.FE.Web.State.ActiveUser;

namespace YptoTaskManager.FE.Web.Components.Pages;

public partial class Login
{
    [Inject]
    private HttpClient HttpClient { get; set; } = new();

    [Inject]
    private IAuthApiClient AuthApiClient { get; set; } = default!;

    [Inject]
    private IAuthTokenProvider AuthTokenProvider { get; set; } = default!;

    [Inject]
    private IDispatcher Dispatcher { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private string _email = string.Empty;
    private string _password = string.Empty;
    private bool _isLoading;
    private string? _errorMessage;

    private async Task LoginAsync()
    {
        _errorMessage = null;

        if (string.IsNullOrWhiteSpace(_email) ||
            string.IsNullOrWhiteSpace(_password))
        {
            _errorMessage = "Email and password are required.";
            return;
        }

        _isLoading = true;

        try
        {
            var response = await AuthApiClient.LoginAsync(
                new LoginRequest(
                    _email,
                    _password));

            AuthTokenProvider.SetToken(response.Token);

            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.Token);

            var user = new UserDto(
                response.UserId,
                response.FirstName,
                response.LastName,
                response.Email,
                response.PhoneNumber,
                response.Role);

            Dispatcher.Dispatch(new SetActiveUserAction(user));

            NavigationManager.NavigateTo("/tasks");
        }
        catch (ApiException ex)
        {
            _errorMessage = ex.StatusCode == StatusCodes.Status401Unauthorized
                ? "Invalid email or password."
                : $"API error {ex.StatusCode}: {ex.Message}";
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
}