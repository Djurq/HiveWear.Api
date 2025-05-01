using HiveWear.Application.Images.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiveWear.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FileStorageController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile image)
        {
            if (image is null)
            {
                return BadRequest();
            }

            string path = await _mediator.Send(new UploadImageCommand(image.OpenReadStream()));

            return Ok(path);
        }
    }
}
