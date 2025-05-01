using HiveWear.Application.Authentication;
using HiveWear.Application.Authentication.Commands;
using HiveWear.Domain.Dto;
using HiveWear.Domain.Models;
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
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (loginModel is null)
            {
                return BadRequest("Login model cannot be null.");
            }

            LoginResult loginResult = await _mediator.Send(new LoginCommand(loginModel)).ConfigureAwait(false);

            if (string.IsNullOrEmpty(loginResult.Token))
            {
                return Unauthorized("Invalid username or password.");
            }

            SetRefreshTokenCookie(loginResult.Token);

            return Ok(loginResult);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if (registerModel is null)
            {
                return BadRequest("Register model cannot be null.");
            }

            string token = await _mediator.Send(new RegisterCommand(registerModel)).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(token))
            {
                SetRefreshTokenCookie(token);
                return Ok(new {Token = token});
            }

            return BadRequest("User registration failed.");
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

            RefreshTokenResult refreshTokenResult = await _mediator.Send(new RefreshTokenCommand(refreshToken)).ConfigureAwait(false);
            string? jwtToken = await _mediator.Send(new JwtTokenCommand()).ConfigureAwait(false);

            if (refreshTokenResult.IsSuccess && !string.IsNullOrEmpty(jwtToken))
            {
                SetRefreshTokenCookie(refreshTokenResult.RefreshToken.Token);
                return Ok(new { Token = jwtToken });
            }

            return Unauthorized("Refresh token is invalid or expired.");
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            CookieOptions cookieOptions = new()
            {
                HttpOnly = true,
                Secure = true, // Alleen via HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
