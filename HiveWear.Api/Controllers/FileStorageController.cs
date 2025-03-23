using HiveWear.Application.Images.Commands;
using HiveWear.Domain.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HiveWear.Api.Controllers
{
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
