using InitialSetupBackend.Shared.Requests;
using InitialSetupBackend.Shared.Responses;

namespace InitialSetupBackend.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponses> AuthenticateAsync(AuthRequest request, CancellationToken cancellationToken);
        Task<TwoFactorResponse> GenerateTwoFactorSecretAsync(int userId, CancellationToken cancellationToken);
        Task DisableTwoFactorSecretAsync(int userId, string token, CancellationToken cancellationToken);
        Task ValidateTwoFactorSecretAsync(int userId, string token, CancellationToken cancellationToken);
    }
}
