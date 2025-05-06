using HiveWear.Application.Interfaces.Repositories;
using HiveWear.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiveWear.Application.Clothing.Commands
{
    public record UpdateClothingItemCommand(ClothingItem? ClothingItem) : IRequest<ClothingItem>
    {
    }

    public class UpdateClothingItemCommandHandler(IClothingRepository clothingRepository) : IRequestHandler<UpdateClothingItemCommand, ClothingItem>
    {
        private readonly IClothingRepository _clothingRepository = clothingRepository ?? throw new ArgumentNullException(nameof(clothingRepository));

        public async Task<ClothingItem> Handle(UpdateClothingItemCommand request, CancellationToken cancellationToken)
        {
            if (request.ClothingItem is null)
            {
                throw new ArgumentNullException("Clothing item is null", nameof(request.ClothingItem));
            }

            return await _clothingRepository.UpdateClothingItemAsync(request.ClothingItem).ConfigureAwait(false);
        }
    }
}
