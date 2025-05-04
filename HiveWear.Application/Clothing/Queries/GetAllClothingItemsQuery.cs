using HiveWear.Application.Interfaces.Repositories;
using HiveWear.Domain.Entities;
using MediatR;

namespace HiveWear.Application.Clothing.Queries
{
    public record GetAllClothingItemsQuery : IRequest<IEnumerable<ClothingItem>>
    {
    }

    public class GetAllClothingItemsQueryHandler(IClothingRepository clothingRepository) : IRequestHandler<GetAllClothingItemsQuery, IEnumerable<ClothingItem>>
    {
        private readonly IClothingRepository _clothingRepository = clothingRepository ?? throw new ArgumentNullException(nameof(clothingRepository));

        public async Task<IEnumerable<ClothingItem>> Handle(GetAllClothingItemsQuery request, CancellationToken cancellationToken)
        {
            return await _clothingRepository.GetAllClothingItemsAsync().ConfigureAwait(false);
        }
    }
}
