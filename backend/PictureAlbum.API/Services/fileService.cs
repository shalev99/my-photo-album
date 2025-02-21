using Microsoft.EntityFrameworkCore;
using PictureAlbum.API.Data;
using PictureAlbum.API.Models;
using PictureAlbum.API.Utils;

namespace PictureAlbum.API.Services
{
    /// <summary>
    /// Service responsible for handling file-related operations.
    /// </summary>
    public class FileService : IFileService
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of <see cref="FileService"/>.
        /// </summary>
        public FileService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Uploads a file after performing security validations.
        /// </summary>
        /// <param name="file">The uploaded file.</param>
        /// <param name="fileName">The user-defined file name.</param>
        /// <param name="fileDate">The optional file date.</param>
        /// <param name="fileDescription">The optional file description.</param>
        /// <returns>The uploaded file entity.</returns>
        public async Task<FileEntity> UploadFileAsync(IFormFile file, string fileName, string? fileDate, string? fileDescription)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file), "Uploaded file cannot be null.");

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be empty.", nameof(fileName));

            // Ensure validation is awaited
            await FileUtils.ValidateFileAsync(file, fileName, _dbContext);

            // Read the file content and generate the base64-encoded version
            var (fileContent, base64FileContent) = await FileUtils.ReadFileContentAsync(file);

            // Create the file entity with validated data
            var fileEntity = new FileEntity
            {
                Name = fileName.Trim(),
                FileName = Path.GetFileName(file.FileName), // Ensures no path traversal attacks
                FileType = file.ContentType,
                FileSize = file.Length,
                FileContent = fileContent,
                FileContentBase64 = base64FileContent,
                Description = fileDescription?.Trim(),
                UploadDate = FileUtils.ParseFileDate(fileDate)
            };

            // Securely save the entity to the database
            await _dbContext.Files.AddAsync(fileEntity);
            await _dbContext.SaveChangesAsync();

            return fileEntity;
        }

        /// <summary>
        /// Retrieves a list of all stored files.
        /// </summary>
        /// <returns>A list of <see cref="FileEntity"/> objects.</returns>
        public async Task<List<FileEntity>> GetFilesAsync()
        {
            return await _dbContext.Files.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Retrieves a paginated list of stored files.
        /// </summary>
        /// <param name="page">The page number (1-based).</param>
        /// <param name="pageSize">The number of files per page.</param>
        /// <returns>A paginated list of <see cref="FileEntity"/> objects.</returns>
        public async Task<List<FileEntity>> GetFilesAsync(int page, int pageSize)
        {
            if (page < 1) page = 1; // Ensure page number is at least 1
            if (pageSize <= 0) pageSize = 10; // Default to 10 if invalid size

            return await _dbContext.Files
                .OrderBy(f => f.UploadDate) // Ensure consistent ordering
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
