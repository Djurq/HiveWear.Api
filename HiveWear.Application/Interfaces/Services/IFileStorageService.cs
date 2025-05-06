namespace HiveWear.Domain.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(string fileName, Stream fileStream);
        Task<bool> DeleteFileAsync(string filePath);
    }
}
