namespace YptoTaskManager.FE.Web.Services.Auth;

public interface IAuthTokenProvider
{
    string? Token { get; }

    void SetToken(string token);

    void ClearToken();
}
