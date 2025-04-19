using HiveWear.Domain.Interfaces.Services;

namespace HiveWear.Infrastructure.Services
{
    internal sealed class FileStorageService : IFileStorageService
    {
        public Task<bool> DeleteFileAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    return Task.FromResult(true);
                }
                catch (Exception)
                {
                    return Task.FromResult(false);
                }
            }
            else
            {
                return Task.FromResult(false);
            }
        }

        public async Task<string> SaveFileAsync(string fileName, Stream fileStream)
        {
            string directoryPath = "C:\\Users\\DjurredeJong\\source\\repos\\Djurq\\HiveWear.Api\\HiveWear.Api\\wwwroot\\images";

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, fileName);

            using FileStream fileStreamOnDisk = new(filePath, FileMode.Create);
            await fileStream.CopyToAsync(fileStreamOnDisk);

            return filePath;
        }
    }
}
