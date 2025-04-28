using HiveWear.Application.Clothing.Commands;
using HiveWear.Application.Clothing.Queries;
using HiveWear.Application.Images.Commands;
using HiveWear.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiveWear.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClothingController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpGet]
        public async Task<IActionResult> GetAllClothingItems()
        {
            IEnumerable<ClothingItem> clothingItems = await _mediator.Send(new GetAllClothingItemsQuery()).ConfigureAwait(false);
            return Ok(clothingItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddClothingItem([FromBody] ClothingItem clothingItem)
        {
            ClothingItem insertedItem = await _mediator.Send(new AddClothingItemCommand(clothingItem)).ConfigureAwait(false);
            return CreatedAtAction(nameof(GetClothingItemById), new { id = insertedItem.Id }, insertedItem);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClothingItemById(int id)
        {
            ClothingItem? clothingItem = await _mediator.Send(new GetClothingItemByIdQuery(id)).ConfigureAwait(false);

            if (clothingItem is null)
            {
                return NotFound();
            }

            return Ok(clothingItem);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClothingItem([FromBody] ClothingItem clothingItem)
        {
            ClothingItem updatedItem = await _mediator.Send(new UpdateClothingItemCommand(clothingItem)).ConfigureAwait(false);

            return Ok(updatedItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClothingItem(int id)
        {
            bool deletedItem = await _mediator.Send(new DeleteClothingItemCommand(id)).ConfigureAwait(false);

            return Ok(deletedItem);
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadClothingItemWithImage([FromForm] IFormFile file, ClothingItem? clothingItem)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            if (clothingItem is null)
            {
                return BadRequest("No clothing item uploaded");
            }

            string path = await _mediator.Send(new UploadImageCommand(file.OpenReadStream())).ConfigureAwait(false);

            clothingItem.ImagePath = path;

            ClothingItem savedClothingItem = await _mediator.Send(new AddClothingItemCommand(clothingItem)).ConfigureAwait(false);

            return Ok(savedClothingItem);
        }
    }
}
