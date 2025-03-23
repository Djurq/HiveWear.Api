using HiveWear.Domain.Interfaces.Repositories;
using HiveWear.Domain.Models;
using MediatR;

namespace HiveWear.Application.Clothing.Commands
{
    public record class AddClothingItemCommand(ClothingItem? ClothingItem) : IRequest<ClothingItem>
    {
    }

    public class AddClothingItemCommandHandler(IClothingRepository clothingRepository) : IRequestHandler<AddClothingItemCommand, ClothingItem>
    {
        private readonly IClothingRepository _clothingRepository = clothingRepository ?? throw new ArgumentNullException(nameof(clothingRepository));

        public async Task<ClothingItem> Handle(AddClothingItemCommand request, CancellationToken cancellationToken)
        {
            if (request.ClothingItem is null)
            {
                throw new ArgumentNullException("Clothing item is null", nameof(request.ClothingItem));
            }

            return await _clothingRepository.AddClothingItemAsync(request.ClothingItem).ConfigureAwait(false);
        }
    }
}
