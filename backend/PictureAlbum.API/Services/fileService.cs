using Microsoft.AspNetCore.Http;
using PictureAlbum.API.Data;  // Correct namespace for ApplicationDbContext
using PictureAlbum.API.Models;
using Microsoft.EntityFrameworkCore;  // This is necessary for ToListAsync
using System.IO;
using System.Threading.Tasks;
using System.Linq;

public class FileService
{
    private readonly ApplicationDbContext _dbContext;

    public FileService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<FileEntity>> GetFilesAsync()
    {
        return await _dbContext.Files.ToListAsync();  // Now, ToListAsync should work
    }

    public async Task<(bool Success, string Message, int? Id)> AddFileAsync(FileEntity fileEntity, IFormFile file)
    {
        try
        {
            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            var filePath = Path.Combine(uploadDirectory, fileEntity.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            _dbContext.Files.Add(fileEntity);
            await _dbContext.SaveChangesAsync();

            return (true, "File uploaded successfully.", fileEntity.Id);
        }
        catch (Exception ex)
        {
            return (false, $"Error uploading file: {ex.Message}", null);
        }
    }
}
