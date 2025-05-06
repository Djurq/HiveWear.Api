using HiveWear.Application.Authentication.Commands;
using HiveWear.Application.Authentication.Queries;
using HiveWear.Application.Authentication.Requests;
using HiveWear.Application.Authentication.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiveWear.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginModel)
        {
            if (loginModel is null)
            {
                return BadRequest("Login model cannot be null.");
            }

            LoginResponse loginResponse = await _mediator.Send(new LoginCommand(loginModel)).ConfigureAwait(false);

            if (string.IsNullOrEmpty(loginResponse.JwtToken))
            {
                return Unauthorized("Invalid username or password.");
            }

            SetRefreshTokenCookie(loginResponse.RefreshToken);

            return Ok(loginResponse);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (registerRequest is null)
            {
                return BadRequest("Register model cannot be null.");
            }

            RegisterResponse registerResponse = await _mediator.Send(new RegisterCommand(registerRequest)).ConfigureAwait(false);

            if (string.IsNullOrEmpty(registerResponse.JwtToken))
            {
                return BadRequest("User registration failed.");
            }

            SetRefreshTokenCookie(registerResponse.RefreshToken);
            return Ok(registerResponse);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            string? refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return Unauthorized("Refresh token is null.");
            }

            RefreshTokenResponse refreshTokenResponse = await _mediator.Send(new RefreshTokenCommand(refreshToken)).ConfigureAwait(false);
            string? jwtToken = await _mediator.Send(new JwtTokenCommand()).ConfigureAwait(false);

            if (refreshTokenResponse.IsSuccess && !string.IsNullOrEmpty(jwtToken))
            {
                SetRefreshTokenCookie(refreshTokenResponse.RefreshToken.Token);
                return Ok(new { Token = jwtToken });
            }

            return Unauthorized("Refresh token is invalid or expired.");
        }

        [AllowAnonymous]
        [HttpGet("me")]
        public async Task<IActionResult> GetUserInfo()
        {
            string? refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return Unauthorized("Refresh token is null.");
            }

            UserInfoResponse userInfoResponse = await _mediator.Send(new UserInfoQuery(refreshToken)).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(userInfoResponse.RefreshToken) && !string.IsNullOrEmpty(userInfoResponse.JwtToken))
            {
                return Ok(userInfoResponse);
            }

            return Unauthorized("User info retrieval failed.");
        }


        private void SetRefreshTokenCookie(string refreshToken)
        {
            CookieOptions cookieOptions = new()
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
