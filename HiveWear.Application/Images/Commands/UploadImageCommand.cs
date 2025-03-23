using HiveWear.Domain.Interfaces.Services;
using MediatR;

namespace HiveWear.Application.Images.Commands
{
    public record UploadImageCommand(Stream Image) : IRequest<string>
    {
    }

    public class UploadImageCommandHandler(IFileStorageService fileStorageService) : IRequestHandler<UploadImageCommand, string>
    {
        private readonly IFileStorageService _fileStorageService = fileStorageService ?? throw new ArgumentNullException(nameof(fileStorageService));
        public async Task<string> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            string fileName = $"{Guid.NewGuid()}.jpg";

            return await _fileStorageService.SaveFileAsync(fileName, request.Image).ConfigureAwait(false);
        }
    }
}
