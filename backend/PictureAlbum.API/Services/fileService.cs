using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PictureAlbum.API.Data;
using PictureAlbum.API.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<FileEntity> UploadFileAsync(IFormFile File, string fileName, string fileDate, string fileDescription)
        {
            if (File == null || File.Length == 0)
            {
                throw new ArgumentException("No file uploaded.");
            }

            var uploadDirectory = Path.Combine(_env.ContentRootPath, "uploads");
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(File.FileName);
            var filePath = Path.Combine(uploadDirectory, uniqueFileName);

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await File.CopyToAsync(stream);
            }

            // Convert file content to a byte array
            byte[] fileContent;
            using (var memoryStream = new MemoryStream())
            {
                await File.CopyToAsync(memoryStream);
                fileContent = memoryStream.ToArray();
            }

            var fileEntity = new FileEntity
            {
                Name = fileName,
                FileName = uniqueFileName,
                FileType = File.ContentType,
                FileSize = File.Length,
                FileContent = fileContent,
                Description = fileDescription,
                UploadDate = DateTime.TryParse(fileDate, out var parsedDate) ? parsedDate : DateTime.UtcNow
            };

            _dbContext.Files.Add(fileEntity);
            await _dbContext.SaveChangesAsync();

            return fileEntity;
        }

        public async Task<List<FileEntity>> GetFilesAsync()
        {
            return await _dbContext.Files.ToListAsync();
        }
    }
}
