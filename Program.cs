using Microsoft.AspNetCore.Authentication.JwtBearer;
using InitialSetupBackend.Shared.Middlewares;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using InitialSetupBackend.Interfaces;
using InitialSetupBackend.Services;
using InitialSetupBackend.Database;
using System.Text;

namespace InitialSetupBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateSlimBuilder(args);

            builder.Services.AddScoped<IAuthService, AuthService>();

            // Cors
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowAllOrigins",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            // Entityframework
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Jwt
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
                };
            });

            // Rate Limiting
            builder.Services.AddRateLimiter(rateLimiting => rateLimiting
               .AddFixedWindowLimiter(policyName: "fixed", options =>
               {
                   options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                   options.Window = TimeSpan.FromMinutes(1);
                   options.PermitLimit = 50;
                   options.QueueLimit = 1;
               }));

            builder.Services.AddLogging();
            builder.Services.AddControllers();
            builder.Services.AddAuthorization();


            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseCors("AllowAllOrigins");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRateLimiter();
            app.MapControllers();
            app.UseRouting();

            app.Run();
        }
    }
}
