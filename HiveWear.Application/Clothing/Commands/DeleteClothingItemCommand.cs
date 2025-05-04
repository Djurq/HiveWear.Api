using HiveWear.Application.Interfaces.Repositories;
using MediatR;

namespace HiveWear.Application.Clothing.Commands
{
    public record DeleteClothingItemCommand(int? Id) : IRequest<bool>
    {
    }

    public class DeleteClothingItemCommandHandler(IClothingRepository clothingRepository) : IRequestHandler<DeleteClothingItemCommand, bool>
    {
        private readonly IClothingRepository _clothingRepository = clothingRepository ?? throw new ArgumentNullException(nameof(clothingRepository));

        public async Task<bool> Handle(DeleteClothingItemCommand request, CancellationToken cancellationToken)
        {
            if (request.Id is null)
            {
                throw new ArgumentNullException("Id is null", nameof(request.Id));
            }

            return await _clothingRepository.DeleteClothingItemAsync(request.Id.Value).ConfigureAwait(false);
        }
    }
}