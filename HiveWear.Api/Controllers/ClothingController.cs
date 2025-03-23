using HiveWear.Application.Clothing.Queries;
using HiveWear.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HiveWear.Api.Controllers
{
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

    }
}
