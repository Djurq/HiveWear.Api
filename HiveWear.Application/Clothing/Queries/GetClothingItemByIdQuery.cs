using HiveWear.Application.Interfaces.Repositories;
using HiveWear.Domain.Entities;
using MediatR;

namespace HiveWear.Application.Clothing.Queries
{
    public record GetClothingItemByIdQuery(int? Id) : IRequest<ClothingItem?>
    {
    }

    public class GetClothingItemByIdQueryHandler(IClothingRepository clothingRepository) : IRequestHandler<GetClothingItemByIdQuery, ClothingItem?>
    {
        private readonly IClothingRepository _clothingRepository = clothingRepository ?? throw new ArgumentNullException(nameof(clothingRepository));

        public async Task<ClothingItem?> Handle(GetClothingItemByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id is null)
            {
                throw new ArgumentNullException("Id is null", nameof(request.Id));
            }

            return await _clothingRepository.GetClothingItemByIdAsync(request.Id.Value).ConfigureAwait(false);
        }
    }
}
