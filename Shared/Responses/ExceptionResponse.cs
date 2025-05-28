using System.Text.Json.Serialization;

namespace InitialSetupBackend.Shared.Responses
{
    public class ExceptionResponse
    {
        public bool ApiOnline { get; set; }
        public string? ErrorMessage { get; set; }
        public int StatusCode { get; set; }
    }
}
