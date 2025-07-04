﻿using HiveWear.Application.Clothing.Commands;
using HiveWear.Application.Clothing.Queries;
using HiveWear.Domain.Entities;
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
    }
}
