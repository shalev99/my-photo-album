using Microsoft.EntityFrameworkCore;
using PictureAlbum.API.Data;
using PictureAlbum.API.Models;
using PictureAlbum.API.Utils;

namespace PictureAlbum.API.Services
{
    public class FileService : IFileService
    {
        private readonly ApplicationDbContext _dbContext;

        // Constructor to inject the database context
        public FileService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Uploads a file after performing security validations.
        /// </summary>
        public async Task<FileEntity> UploadFileAsync(IFormFile file, string fileName, string? fileDate, string? fileDescription)
        {
            // Validate input before proceeding
            FileUtils.ValidateFile(file, fileName, _dbContext);

            // Read the file content and prepare the base64-encoded version
            var (fileContent, base64FileContent) = await FileUtils.ReadFileContentAsync(file);

            // Prepare the entity before inserting it into the database
            var fileEntity = new FileEntity
            {
                Name = fileName.Trim(), // Trim to remove extra spaces
                FileName = file.FileName,
                FileType = file.ContentType,
                FileSize = file.Length,
                FileContent = fileContent,
                FileContentBase64 = base64FileContent,
                Description = fileDescription?.Trim(), // Optionally trim the description
                UploadDate = FileUtils.ParseFileDate(fileDate) // Parse the date string to a DateTime object
            };

            // Securely save to the database
            _dbContext.Files.Add(fileEntity);
            await _dbContext.SaveChangesAsync(); // Save the changes to the database

            return fileEntity; // Return the uploaded file entity
        }

        /// <summary>
        /// Retrieves a list of all stored files.
        /// </summary>
        public async Task<List<FileEntity>> GetFilesAsync()
        {
            // Return all files from the database without tracking changes
            return await _dbContext.Files.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Retrieves a paginated list of stored files based on page and page size.
        /// </summary>
        public async Task<List<FileEntity>> GetFilesAsync(int page, int pageSize)
        {
            // Perform pagination using Skip (to skip files from previous pages) and Take (to limit the number of files per page)
            return await _dbContext.Files
                .Skip((page - 1) * pageSize) // Skip files for previous pages
                .Take(pageSize) // Take the specified number of files for the current page
                .AsNoTracking() // Do not track changes (improves performance for read-only operations)
                .ToListAsync(); // Execute the query and return the results as a list
        }
    }
}
