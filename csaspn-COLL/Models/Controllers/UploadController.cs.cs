using csaspn_COLL.Models;
using csaspn_COLL.Services;
using Microsoft.AspNetCore.Mvc;
using csaspn_COLL.Models;
using csaspn_COLL.Services;

namespace csaspn_COLL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly ImageService _imageService;
        private readonly string _uploadsPath;
        private readonly ILogger<UploadController> _logger;

        public UploadController(IConfiguration config, ILogger<UploadController> logger)
        {
            _logger = logger;
            _uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(_uploadsPath);

            var connectionString = config.GetConnectionString("DefaultConnection");
            _imageService = new ImageService(connectionString);
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest(new { success = false, message = "Файл не получен" });

                const long maxFileSize = 5 * 1024 * 1024; // 5MB
                if (file.Length > maxFileSize)
                    return BadRequest(new { success = false, message = "Размер файла превышает 5 MB" });

                var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
                if (!allowedTypes.Contains(file.ContentType))
                    return BadRequest(new { success = false, message = "Недопустимый формат файла" });

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(_uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var image = new ImageFile
                {
                    FileName = file.FileName,
                    FilePath = $"/uploads/{fileName}",
                    FileSize = file.Length,
                    UploadDate = DateTime.Now
                };

                _imageService.SaveImage(image);

                return Ok(new { success = true, message = "Файл успешно загружен!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке файла");
                return StatusCode(500, new { success = false, message = "Ошибка сервера при загрузке файла" });
            }
        }

        [HttpGet]
        public IActionResult GetImages()
        {
            try
            {
                var images = _imageService.GetAllImages();
                return Ok(new { success = true, data = images });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка изображений");
                return StatusCode(500, new { success = false, message = "Ошибка сервера" });
            }
        }
    }
}
