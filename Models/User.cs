using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InitialSetupBackend.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Role { get; set; }
        public bool IsBlocked { get; set; } = false;
        public string? BlockedReason { get; set; }
        public string? Name { get; set; }
        public string? HashPassword { get; set; }
        public string? SecretTwoFactor { get; set; }
        public bool ActivatedTwoFactor { get; set; } = false;

        public string? Email { get; set; }
        public bool EmailVerified { get; set; } = false;
        public DateTimeOffset? LastEmailVerifiedAt { get; set; } = null;
        public string? Phone { get; set; }
        public bool PhoneVerified { get; set; } = false;
        public DateTimeOffset? LastPhoneVerifiedAt { get; set; } = null;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
