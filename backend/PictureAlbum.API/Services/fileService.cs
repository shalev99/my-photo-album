using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PictureAlbum.API.Data;
using PictureAlbum.API.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PictureAlbum.API.Models;
using PictureAlbum.API.DTOs;  // Updated reference to the new namespace

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
        public async Task<FileDTO> UploadFileAsync(
            IFormFile file, 
            string fileName, 
            string fileDate, 
            string fileDescription)
        {
            // Validate the file input
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded.");
            }

            // Check if the file name already exists in the database
            if (_dbContext.Files.Any(f => f.Name == fileName))
            {
                throw new ArgumentException("A file with this name already exists.");
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
                FileName = fileName,
                FileType = file.ContentType,
                FileSize = file.Length,
                FileContent = fileContent, // Store the byte array in the DB for later retrieval
                Description = fileDescription,
                UploadDate = DateTime.TryParse(fileDate, out var parsedDate)
                    ? parsedDate
                    : DateTime.UtcNow
            };

            // Add the file entity to the database
            _dbContext.Files.Add(fileEntity);
            await _dbContext.SaveChangesAsync();

            // Create a FileDTO response object
            var fileDTO = new FileDTO
            {
                Id = fileEntity.Id,
                Name = fileEntity.Name,
                FileName = fileEntity.FileName,
                FileSize = fileEntity.FileSize,
                FileType = fileEntity.FileType,
                FileContentBase64 = base64FileContent, // Include base64 content in the response
                Description = fileEntity.Description,
                UploadDate = fileEntity.UploadDate,
                Src = $"data:{fileEntity.FileType};base64,{base64FileContent}"
            };

            // Return the fileDTO to the client
            return fileDTO;
        }

        // Method to retrieve all files
        public async Task<List<FileEntity>> GetFilesAsync()
        {
            return await _dbContext.Files.ToListAsync();
        }
    }
}
