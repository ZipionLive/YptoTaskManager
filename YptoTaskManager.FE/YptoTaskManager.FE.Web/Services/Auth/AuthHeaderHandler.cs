using System.Net.Http.Headers;

namespace YptoTaskManager.FE.Web.Services.Auth;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly IAuthTokenProvider _authTokenProvider;

    public AuthHeaderHandler(IAuthTokenProvider authTokenProvider)
    {
        _authTokenProvider = authTokenProvider;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(_authTokenProvider.Token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    _authTokenProvider.Token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}