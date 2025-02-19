using Microsoft.EntityFrameworkCore;
using PictureAlbum.API.Data;
using PictureAlbum.API.Models;
using PictureAlbum.API.Utils;


namespace PictureAlbum.API.Services
{
    public class FileService : IFileService
    {
        private readonly ApplicationDbContext _dbContext;

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

            var (fileContent, base64FileContent) = await FileUtils.ReadFileContentAsync(file);

            // Prepare the entity before inserting it into the database
            var fileEntity = new FileEntity
            {
                Name = fileName.Trim(),
                FileName = file.FileName,
                FileType = file.ContentType,
                FileSize = file.Length,
                FileContent = fileContent,
                FileContentBase64 = base64FileContent,
                Description = fileDescription?.Trim(),
                UploadDate = FileUtils.ParseFileDate(fileDate)
            };

            // Securely save to the database
            _dbContext.Files.Add(fileEntity);
            await _dbContext.SaveChangesAsync();

            return fileEntity;
        }

        /// <summary>
        /// Retrieves a list of stored files.
        /// </summary>
        public async Task<List<FileEntity>> GetFilesAsync()
        {
            return await _dbContext.Files.AsNoTracking().ToListAsync();
        }
    }
}
