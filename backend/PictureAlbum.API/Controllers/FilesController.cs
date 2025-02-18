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
        public async Task<IActionResult> UploadFile([FromForm] IFormFile pictureFile, [FromForm] string pictureName, [FromForm] string pictureDate, [FromForm] string pictureDescription)
        {
            try
            {
                var fileEntity = await _fileService.UploadFileAsync(pictureFile, pictureName, pictureDate, pictureDescription);
                return Ok(new { fileEntity.Id, fileEntity.Name, fileEntity.FileName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while uploading the file.", Error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFiles()
        {
            try
            {
                var files = await _fileService.GetFilesAsync();
                return Ok(files);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving files.", Error = ex.Message });
            }
        }
    }
}
