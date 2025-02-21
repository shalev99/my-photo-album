using Microsoft.AspNetCore.Mvc;
using PictureAlbum.API.Models;
using PictureAlbum.API.Services;
using System.Threading.Tasks;

namespace PictureAlbum.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile File, [FromForm] string fileName, [FromForm] string? fileDate, [FromForm] string? fileDescription)
        {
            try
            {
                var fileEntity = await _fileService.UploadFileAsync(File, fileName, fileDate, fileDescription);
                return Ok(fileEntity);            
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while uploading the file.", Error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFiles([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var files = await _fileService.GetFilesAsync(page, pageSize);
                return Ok(files);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving files.", Error = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllFiles()
        {
            try
            {
                var files = await _fileService.GetFilesAsync();
                return Ok(files);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving all files", Error = ex.Message });
            }
        }
    }
}
