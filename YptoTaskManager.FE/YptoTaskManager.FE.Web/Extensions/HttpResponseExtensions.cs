using YptoTaskManager.FE.Web.Services;

namespace YptoTaskManager.FE.Web.Extensions;

public static class HttpResponseExtensions
{
    public static async Task EnsureSuccessAsync(this HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode) { return; }

        var message = await response.Content.ReadAsStringAsync(cancellationToken);

        throw new ApiException(
            (int)response.StatusCode,
            string.IsNullOrWhiteSpace(message)
                ? response.ReasonPhrase ?? "API error."
                : message);
    }
}
