using csaspn_COLL.Services;
using Microsoft.AspNetCore.Mvc;
using csaspn_COLL.Services;

[Route("api/[controller]")]
[ApiController]
public class UploadController : ControllerBase
{
    private readonly ImageService _imageService;

    public UploadController(IConfiguration config)
    {
        _imageService = new ImageService(config.GetConnectionString("DefaultConnection"));
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { success = false, message = "Файл не получен" });
        if (file.Length > 5 * 1024 * 1024)
            return BadRequest(new { success = false, message = "Файл больше 5МБ" });

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest(new { success = false, message = "Недопустимый формат файла" });

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var image = new ImageData
        {
            FileContent = ms.ToArray(),
            FileName = file.FileName
        };
        _imageService.SaveImage(image);

        return Ok(new { success = true, message = "Файл успешно загружен в SQL Server!" });
    }

    [HttpGet("{id}")]
    public IActionResult GetImage(int id)
    {
        var image = _imageService.GetImageById(id);
        if (image == null)
            return NotFound();

        var contentType = GetContentType(image.FileName);
        return File(image.FileContent, contentType);
    }

    [HttpGet("all")]
    public IActionResult GetAllImages()
    {
        var images = _imageService.GetAllImages();
        return Ok(images);
    }

    private string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ => "application/octet-stream",
        };
    }
}
