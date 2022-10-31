using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApi.Models.Input;

namespace SocialMediaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<ActionResult<string>> UploadImage([FromForm] UserFileUploadInput input)
        {
            //  TODO: Authenticate user

            if (input.File.Length <= 0)
            {
                return BadRequest(input.File.FileName);
            }

            //  TODO: figure out why contenttype is always null
            //if (!input.File.ContentType.Equals("image/jpg", StringComparison.OrdinalIgnoreCase) &&
            //    !input.File.ContentType.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase) &&
            //    !input.File.ContentType.Equals("image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
            //    !input.File.ContentType.Equals("image/gif", StringComparison.OrdinalIgnoreCase) &&
            //    !input.File.ContentType.Equals("image/png", StringComparison.OrdinalIgnoreCase))
            //{
            //    return BadRequest("Incorrect File Type");
            //}

            string extension = Path.GetExtension(input.File.FileName);
            if (!extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) &&
                !extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase) &&
                !extension.Equals(".gif", StringComparison.OrdinalIgnoreCase) &&
                !extension.Equals(".png", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Incorrect File Type");
            }

            string fileName = Guid.NewGuid().ToString() + extension;

            var folderPath = Path.Combine("Images", "Uploads");
            var fullFolderPath = Path.Combine(_webHostEnvironment.ContentRootPath, folderPath);
            var filePath = Path.Combine(fullFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await input.File.CopyToAsync(stream);
            }

            var relativePath = Path.Combine(folderPath, fileName);

            return Ok(relativePath);
        }
    }
}
