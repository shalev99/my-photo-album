using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/pictures")]
public class PicturesController : ControllerBase
{
    private readonly PictureService _service;
    public PicturesController(PictureService service) { _service = service; }

    [HttpGet]
    public async Task<IActionResult> GetPictures() => Ok(await _service.GetPicturesAsync());

    [HttpPost]
    public async Task<IActionResult> AddPicture([FromBody] Picture picture)
    {
        var result = await _service.AddPictureAsync(picture);
        return result.Success ? Ok(new { Id = result.Id }) : BadRequest(result.Message);
    }
}
