using Microsoft.AspNetCore.Mvc.RazorPages;
using csaspn_COLL.Services;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace csaspn_COLL.Pages
{
    public class ViewImagesModel : PageModel
    {
        private readonly ImageService _imageService;

        public List<ImageData> Images { get; set; } = new();

        public ViewImagesModel(IConfiguration config)
        {
            _imageService = new ImageService(config.GetConnectionString("DefaultConnection"));
        }

        public void OnGet()
        {
            Images = _imageService.GetAllImages();
        }
    }
}
