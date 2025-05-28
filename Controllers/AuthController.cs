using InitialSetupBackend.Shared.Responses;
using InitialSetupBackend.Shared.Requests;
using System.IdentityModel.Tokens.Jwt;
using InitialSetupBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InitialSetupBackend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] AuthRequest request, CancellationToken cancellationToken)
        {
            var token = await authService.AuthenticateAsync(request, cancellationToken);
            return Ok(token);
        }

        [HttpGet("validate")]
        public async Task<IActionResult> ValidateUserToken()
        {
            bool isAuthenticated = User.Identity != null && await Task.FromResult(User.Identity.IsAuthenticated);

            var jwtToken = new JwtSecurityTokenHandler()
                .ReadJwtToken(HttpContext.Request.Headers["Authorize"]);

            if (isAuthenticated)
                return Ok(new InformationResponse($"Valid jwt bearer token, expires in: {jwtToken.ValidTo}", true));
            else
                return Ok(new InformationResponse("Invalid Jwt Token", false));
        }
    }
}
