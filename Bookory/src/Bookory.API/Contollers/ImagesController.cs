using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookory.API.Contollers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ImagesController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult GetImage([FromQuery] string path)
        {
            // Combine the wwwroot path and the provided path to get the full image path.
            string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, path);

            // Check if the file exists.
            if (System.IO.File.Exists(imagePath))
            {
                // Read the image file and return it with the appropriate content type.
                byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                return File(imageBytes, "image/jpeg"); // Adjust the content type based on your image type.
            }
            else
            {
                return NotFound(); // Return a 404 Not Found if the image doesn't exist.
            }
        }
    }
}
