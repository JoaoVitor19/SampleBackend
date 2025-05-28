using System.Net;

namespace InitialSetupBackend.Shared.Exceptions
{
    public class NotFoundException(string? message) : Exception(message)
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.NotFound;
    }
}
