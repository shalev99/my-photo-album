using Microsoft.AspNetCore.Mvc;
using PictureAlbum.API.Models;  // Correct namespace for FileEntity
using PictureAlbum.API.Data;  // Correct namespace for ApplicationDbContext
using Microsoft.AspNetCore.Http;  // To use IFormFile

[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly FileService _service;

    public FilesController(FileService service) 
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetFiles() 
    {
        var files = await _service.GetFilesAsync();
        return Ok(files);
    }

    [HttpPost]
    public async Task<IActionResult> AddFile([FromForm] IFormFile file, [FromForm] string description)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var fileEntity = new FileEntity
        {
            Name = file.FileName,
            FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName),
            FileType = file.ContentType,
            FileSize = file.Length,
            Description = description,
            UploadDate = DateTime.Now
        };

        var result = await _service.AddFileAsync(fileEntity, file);
        return result.Success ? Ok(new { Id = result.Id, Name = fileEntity.Name }) : BadRequest(result.Message);
    }
}
