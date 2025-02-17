using Microsoft.AspNetCore.Mvc;
using PictureAlbum.API.Models; // Add this namespace for FileEntity

[ApiController]
[Route("api/files")]  // Changed route from "pictures" to "files"
public class FilesController : ControllerBase
{
    private readonly FileService _service; // Changed to FileService
    public FilesController(FileService service) { _service = service; }

    [HttpGet]
    public async Task<IActionResult> GetFiles() => Ok(await _service.GetFilesAsync());

    [HttpPost]
    public async Task<IActionResult> AddFile([FromBody] FileEntity fileEntity) // Changed to FileEntity
    {
        var result = await _service.AddFileAsync(fileEntity); // Changed to AddFileAsync
        return result.Success ? Ok(new { Id = result.Id }) : BadRequest(result.Message);
    }
}
