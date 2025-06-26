using InitialSetupBackend.Shared.Exceptions;
using InitialSetupBackend.Shared.Responses;
using System.Net;

namespace InitialSetupBackend.Shared.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate _next, ILogger<ExceptionMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                var (errorMessage, statusCode) = HandleException(ex);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                if (statusCode == 500)
                {
                    string? logErrorSource = ex.Source;
                    string? logErrorMessage = ex.Message;
                    string? logErrorStackTrace = ex.StackTrace;
                    string? logErrorInnerException = ex.InnerException?.ToString();
                    logger.LogError("NotTratedError = Message = {Message}, StackTrace = {StackTrace}, InnerException = {InnerException}, Source = {Source}", logErrorMessage, logErrorStackTrace, logErrorInnerException, logErrorSource);
                }

                var response = new ExceptionResponses
                {
                    ApiOnline = true,
                    StatusCode = statusCode,
                    ErrorMessage = errorMessage
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }

        public (string ErrorMessage, int StatusCode) HandleException(Exception ex)
        {
            return ex switch
            {
                BadRequestException badRequestEx => (badRequestEx.Message, (int)badRequestEx.StatusCode),
                NotFoundException userNotFoundEx => (userNotFoundEx.Message, (int)userNotFoundEx.StatusCode),
                UnauthorizedException unauthorizedException => (unauthorizedException.Message, (int)unauthorizedException.StatusCode),
                NullReferenceException nullReferenceException => (nullReferenceException.Message, 500),
                _ => (ex.Message, (int)HttpStatusCode.InternalServerError) // Handle others exceptions;
            };
        }
    }
}
