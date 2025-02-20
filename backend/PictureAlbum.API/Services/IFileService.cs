using PictureAlbum.API.Models;

namespace PictureAlbum.API.Services
{
    public interface IFileService
    {
        Task<FileEntity> UploadFileAsync(IFormFile file, string fileName, string? fileDate, string? fileDescription);
        
        // Retrieves a list of all stored files
        Task<List<FileEntity>> GetFilesAsync();
        
        // Retrieves a paginated list of stored files, based on the provided page number and page size
        Task<List<FileEntity>> GetFilesAsync(int page, int pageSize);
    }
}
