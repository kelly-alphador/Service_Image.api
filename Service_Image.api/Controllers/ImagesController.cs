using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Service_Image.Api.Domaine.Core.DTO;
using Service_Image.Api.Domaine.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Service_Image.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        private readonly IImageService _imageService;
        private readonly ILogger<ImagesController> _logger;

        public ImagesController(
            IImageService imageService,
            ILogger<ImagesController> logger)
        {
            _imageService = imageService;
            _logger = logger;
        }

        /// <summary>
        /// Upload une nouvelle image
        /// </summary>
        [HttpPost]
        [RequestSizeLimit(10_000_000)] // 10MB max
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var result = await _imageService.UploadImageAsync(file,userId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid image upload");
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image");
                return StatusCode(500, new { Error = "Internal server error" });
            }
        }

        /// <summary>
        /// Applique des transformations à une image
        /// </summary>
        [HttpPost("{id}/transform")]
        [Authorize] // Sécurisé
        public async Task<IActionResult> TransformImage(Guid id,[FromBody] ImageTransformationRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _imageService.TransformImageAsync(id, request);

                // Cache-Control pour les images transformées
                Response.Headers.Add("Cache-Control", "public,max-age=86400"); // 24h cache

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error transforming image {id}");
                return StatusCode(500, new { Error = "Internal server error" });
            }
        }

        /// <summary>
        /// Récupère les détails d'une image
        /// </summary>
        [HttpGet("{id}")]
        [ResponseCache(Duration = 30)] // Cache 30s
        public async Task<IActionResult> GetImage(Guid id)
        {
            try
            {
                var result = await _imageService.GetImageAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting image {id}");
                return StatusCode(500, new { Error = "Internal server error" });
            }
        }

        /// <summary>
        /// Liste paginée des images
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetImages([FromQuery] int page = 1,[FromQuery] int limit = 10)
        {
            try
            {
                if (page < 1) page = 1;
                if (limit < 1 || limit > 100) limit = 10;

                var result = await _imageService.GetImagesAsync(page, limit);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting images list");
                return StatusCode(500, new { Error = "Internal server error" });
            }
        }
    }
}

