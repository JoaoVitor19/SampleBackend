namespace InitialSetupBackend.Shared.Requests
{
    public class AuthRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Token { get; set; }
    }
}
