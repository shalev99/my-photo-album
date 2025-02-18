using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PictureAlbum.API.Data;
using PictureAlbum.API.Models;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace PictureAlbum.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public FileController(ApplicationDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        // Uploads a file to the server and saves metadata to the database
        
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile pictureFile, [FromForm] string pictureName, [FromForm] string pictureDate, [FromForm] string pictureDescription)
        {
            if (pictureFile == null || pictureFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Define the directory path for file storage
            var uploadDirectory = Path.Combine(_env.ContentRootPath, "uploads");
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            // Generate a unique file name
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(pictureFile.FileName);
            var filePath = Path.Combine(uploadDirectory, uniqueFileName);

            try
            {
                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await pictureFile.CopyToAsync(stream);
                }

                // Convert file content to a byte array
                byte[] fileContent;
                using (var memoryStream = new MemoryStream())
                {
                    await pictureFile.CopyToAsync(memoryStream);
                    fileContent = memoryStream.ToArray();
                }

                // Create a file entity for saving to the database
                var fileEntity = new FileEntity
                {
                    Name = pictureName,
                    FileName = uniqueFileName,
                    FileType = pictureFile.ContentType,
                    FileSize = pictureFile.Length,
                    FileContent = fileContent,
                    Description = pictureDescription,
                    UploadDate = DateTime.TryParse(pictureDate, out var parsedDate) ? parsedDate : DateTime.UtcNow
                };

                // Save file info to the database
                _dbContext.Files.Add(fileEntity);
                await _dbContext.SaveChangesAsync();

                return Ok(new { fileEntity.Id, fileEntity.Name, fileEntity.FileName });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Message = "An error occurred while uploading the file.", Error = ex.Message });
            }
        }

        // Retrieves the list of uploaded files
        [HttpGet]
        public async Task<IActionResult> GetFiles()
        {
            try
            {
                var files = await _dbContext.Files.ToListAsync();
                return Ok(files);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Message = "An error occurred while retrieving files.", Error = ex.Message });
            }
        }
    }
}
