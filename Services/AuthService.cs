using InitialSetupBackend.Shared.Exceptions;
using InitialSetupBackend.Shared.Responses;
using InitialSetupBackend.Shared.Requests;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using InitialSetupBackend.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using InitialSetupBackend.Database;
using InitialSetupBackend.Models;
using System.Security.Claims;
using System.Text;
using OtpNet;

namespace InitialSetupBackend.Services
{
    public class AuthService(AppDbContext context, IConfiguration configuration) : IAuthService
    {
        public async Task<AuthResponse> AuthenticateAsync(AuthRequest request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => !string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Equals(request.Email.ToLower()), cancellationToken)
                ?? throw new UnauthorizedException("Invalid Credêntials");

            if (user.IsBlocked)
            {
                var blockMessage = string.IsNullOrEmpty(user.BlockedReason) ? "Not provided." : user.BlockedReason;
                throw new UnauthorizedException($"Reason: {blockMessage}");
            }

            var hasher = new PasswordHasher<User>();
            var verifyPasswordHashResult = hasher.VerifyHashedPassword(user, user.HashPassword!, request.Password);

            if (verifyPasswordHashResult != PasswordVerificationResult.Success)
            {
                throw new UnauthorizedException("Invalid Credêntials");
            }

            if (user.ActivatedTwoFactor && (string.IsNullOrEmpty(request.Token) || request.Token.Length != 6))
            {
                throw new UnauthorizedException("Provide two factor six digits code");
            }

            if (user.ActivatedTwoFactor && !ValidateToken(user.SecretTwoFactor!, request.Token!))
            {
                throw new UnauthorizedException("Provided two factor is invalid or expired");
            }

            var authClaims = new List<Claim>
                {
                    new ("UserId", user.Id.ToString()),
                    new (ClaimTypes.Email, user.Email!),
                    new (ClaimTypes.Role, user.Role!),
                    new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            var token = GenerateJwtToken(authClaims);

            return new AuthResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresIn = token.ValidTo,
                Role = user.Role
            };
        }

        public async Task<TwoFactorResponse> GenerateTwoFactorSecretAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
                ?? throw new NotFoundException("User not found");

            var secretKey = user.SecretTwoFactor;

            if (user.ActivatedTwoFactor is false)
            {
                if (string.IsNullOrEmpty(secretKey))
                {
                    var newSecretKey = GenerateSecretKey();
                    user.SecretTwoFactor = newSecretKey;
                    await context.SaveChangesAsync(cancellationToken);
                    secretKey = newSecretKey;
                }

                var otpAuth = GenerateOtpAuthUrl(secretKey, user.Email!);

                return new TwoFactorResponse
                {
                    SecretKey = secretKey
                };
            }
            else throw new BadRequestException("Your secret is already created");
        }

        public async Task ValidateTwoFactorSecretAsync(int userId, string token, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(token) || token.Length != 6)
                throw new BadRequestException("Provide token of six digits");

            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
                ?? throw new NotFoundException("User not found");

            if (user.ActivatedTwoFactor == true)
                throw new BadRequestException("Your secret is already activated");

            bool isValidSecretToken = ValidateToken(user.SecretTwoFactor!, token);
            if (!isValidSecretToken)
                throw new BadRequestException("Provided six digits token is invalid");

            user.ActivatedTwoFactor = true;
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task DisableTwoFactorSecretAsync(int userId, string token, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
                ?? throw new NotFoundException("User not found");

            if (user.ActivatedTwoFactor is false)
                throw new BadRequestException("User not activated two factor in account");

            bool isValidtoken = ValidateToken(user.SecretTwoFactor!, token);
            if (!isValidtoken)
                throw new BadRequestException("Provided six digits token is invalid");

            user.ActivatedTwoFactor = false;
            await context.SaveChangesAsync(cancellationToken);
        }

        private JwtSecurityToken GenerateJwtToken(List<Claim> authClaims)
        {
            string jwtIssuer = configuration["Jwt:Issuer"] ?? throw new Exception("Server not configured correctly");
            string jwtSecret = configuration["Jwt:Secret"] ?? throw new Exception("Server not configured correctly");
            string jwtAudience = configuration["Jwt:Audience"] ?? throw new Exception("Server not configured correctly");

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

            var secretExpiresIn = configuration["Jwt:ExpiresIn"]!;
            var expiresIn = DateTime.Now.AddHours(!string.IsNullOrEmpty(secretExpiresIn) ? int.Parse(secretExpiresIn) : 3);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                expires: expiresIn,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

        private static bool ValidateToken(string secretKey, string token)
        {
            var key = Base32Encoding.ToBytes(secretKey);
            var totp = new Totp(key);
            return totp.VerifyTotp(token, out _, VerificationWindow.RfcSpecifiedNetworkDelay);
        }

        private static string GenerateSecretKey()
        {
            var key = KeyGeneration.GenerateRandomKey(20);
            return Base32Encoding.ToString(key);
        }

        private string GenerateOtpAuthUrl(string secretKey, string userEmail)
        {
            string issuer = configuration["Jwt:Issuer"]!;
            return $"otpauth://totp/{issuer}:{userEmail}?secret={secretKey}&issuer={issuer}&digits=6";
        }
    }
}
