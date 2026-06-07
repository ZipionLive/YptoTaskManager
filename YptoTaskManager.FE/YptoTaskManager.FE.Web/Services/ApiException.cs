namespace YptoTaskManager.FE.Web.Services
{
    public class ApiException : Exception
    {
        public int StatusCode { get; init; }

        public ApiException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
