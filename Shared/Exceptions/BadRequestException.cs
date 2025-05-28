using System.Net;

namespace InitialSetupBackend.Shared.Exceptions
{
    public class BadRequestException(string? message) : Exception(message)
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;
    }
}
