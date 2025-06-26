namespace InitialSetupBackend.Shared.Responses
{
    public class AuthResponses
    {
        public string? AccessToken { get; set; }
        public DateTime ExpiresIn { get; set; }
        public string? Role { get; set; }
    }

    public class TwoFactorResponse
    {
        public string? SecretKey { get; set; }
    }
}
