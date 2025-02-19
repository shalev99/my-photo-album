using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PictureAlbum.API.Data;
using PictureAlbum.API.Models;

namespace PictureAlbum.API.Services
{
    public class FileService : IFileService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public FileService(ApplicationDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        // Method for uploading a file
        public async Task<FileEntity> UploadFileAsync(
            IFormFile file, 
            string fileName, 
            string? fileDate, 
            string? fileDescription)
        {
            // Validate the file input
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded.");
            }

            // Check if the file name (actual uploaded file name) already exists in the database
            if (_dbContext.Files.Any(f => f.FileName == file.FileName))
            {
                throw new ArgumentException("A file with this name already exists.");
            }

            // Check if the picture name (from the form) already exists in the database
            if (_dbContext.Files.Any(f => f.Name == fileName))
            {
                throw new ArgumentException("A picture with this name already exists.");
            }


            // Read the file content as a byte array
            byte[] fileContent;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileContent = memoryStream.ToArray();
            }

            // Convert the byte array to base64 string
            string base64FileContent = Convert.ToBase64String(fileContent);

            // Create a new file entity
            var fileEntity = new FileEntity
            {
                Name = fileName,
                FileName = file.FileName,
                FileType = file.ContentType,
                FileSize = file.Length,
                FileContent = fileContent,
                FileContentBase64 = base64FileContent,
                Description = fileDescription,
                UploadDate = DateTime.TryParse(fileDate, out var parsedDate)
                    ? parsedDate
                    : DateTime.UtcNow
            };

            // Add the file entity to the database
            _dbContext.Files.Add(fileEntity);
            await _dbContext.SaveChangesAsync();

            // Return the fileDTO to the client
            return fileEntity;
        }

        // Method to retrieve all files
        public async Task<List<FileEntity>> GetFilesAsync()
        {
            return await _dbContext.Files.ToListAsync();
        }
    }
}
