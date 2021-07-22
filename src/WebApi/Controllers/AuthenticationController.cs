using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Application.Common.Models;
using Blogpost.WebApi.Attributes;
using Blogpost.WebApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Blogpost.WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Аутентификация с помощью email и пароля
        /// </summary>
        [HttpPost]
        [Route("login/email")]
        [ValidateModelState]
        [SwaggerOperation("EmailLogin")]
        [SwaggerResponse(statusCode: 200, type: typeof(TokenResponse), description: "Success")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> EmailLogin([FromBody] EmailLoginRequest body, CancellationToken cancellationToken = default)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
            var parameter = new EmailAuthenticatorParameter { Email = body.Email, Password = body.Password, IpAddress = ipAddress };
            var result = await _authenticationService.Login(parameter, cancellationToken);

            return FromAuthenticationResult(result);
        }

        /// <summary>
        /// Аутентификация с помощью токена
        /// </summary>
        [HttpPost]
        [Route("login/facebook")]
        [ValidateModelState]
        [SwaggerOperation("FacebookLogin")]
        [SwaggerResponse(statusCode: 200, type: typeof(TokenResponse), description: "Success")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> FacebookLogin([FromBody] FacebookLoginRequest body, CancellationToken cancellationToken = default)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
            var parameter = new FacebookAuthenticatorParameter { Token = body.Token, IpAddress = ipAddress };
            var result = await _authenticationService.Login(parameter, cancellationToken);

            return FromAuthenticationResult(result);
        }

        /// <summary>
        /// Аутентификация с помощью токена
        /// </summary>
        [HttpPost]
        [Route("login/google")]
        [ValidateModelState]
        [SwaggerOperation("GoogleLogin")]
        [SwaggerResponse(statusCode: 200, type: typeof(TokenResponse), description: "Success")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest body, CancellationToken cancellationToken = default)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
            var parameter = new GoogleAuthenticatorParameter { Token = body.Token, IpAddress = ipAddress };
            var result = await _authenticationService.Login(parameter, cancellationToken);

            return FromAuthenticationResult(result);
        }

        private IActionResult FromAuthenticationResult(AuthenticationResult result)
        {
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Aggregate((curr, next) => curr + ", " + next));
            }

            return Ok(new TokenResponse
            {
                AccessToken = result.AccessToken,
                ExpiresAt = result.ExpiresAt,
                RefreshToken = result.RefreshToken
            });
        }

        /// <summary>
        /// Обновление access и refresh токенов
        /// </summary>
        [HttpPost]
        [Route("refreshtoken")]
        [ValidateModelState]
        [SwaggerOperation("RefreshToken")]
        [SwaggerResponse(statusCode: 200, type: typeof(TokenResponse), description: "Success")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest body, CancellationToken cancellationToken = default)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
            var result = await _authenticationService.RefreshToken(body.RefreshToken, ipAddress, cancellationToken);

            return FromAuthenticationResult(result);
        }
    }
}
