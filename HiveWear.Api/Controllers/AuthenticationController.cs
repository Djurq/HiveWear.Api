using HiveWear.Application.Authentication;
using HiveWear.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HiveWear.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (loginModel is null)
            {
                return BadRequest("Login model cannot be null.");
            }

            string token = await _mediator.Send(new LoginCommand(loginModel)).ConfigureAwait(false);

            return Ok(new { Token = token });
        }

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
                return Ok(new {Token = token});
            }

            return BadRequest("User registration failed.");
        }
    }
}
