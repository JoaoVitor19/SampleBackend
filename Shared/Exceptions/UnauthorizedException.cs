using System.Net;

namespace InitialSetupBackend.Shared.Exceptions
{
    public class UnauthorizedException(string? message) : Exception(message)
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.Unauthorized;
    }
}
