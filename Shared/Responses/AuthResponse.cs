namespace InitialSetupBackend.Shared.Responses
{
    public class AuthResponse
    {
        public string? AccessToken { get; set; }
        public DateTime ExpiresIn { get; set; }
        public string? Role { get; set; }
    }
}
