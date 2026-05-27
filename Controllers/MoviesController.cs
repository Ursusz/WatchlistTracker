namespace Watchlist_Tracker.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Watchlist_Tracker.Data;
using Watchlist_Tracker.DTOs;
using Watchlist_Tracker.Mappings;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private static readonly HashSet<string> AllowedImageContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/jpg",
        "image/pjpeg",
        "image/png",
        "image/x-png",
        "image/webp"
    };
    private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };

    public MoviesController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
    {
        var movies = await _context.Movies
            .Include(m => m.Category)
            .Include(m => m.Reviews)
            .OrderBy(m => m.Title)
            .ToListAsync();

        return Ok(movies.ToDtoList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDto>> GetMovie(int id)
    {
        var movie = await _context.Movies
            .Include(m => m.Category)
            .Include(m => m.Reviews)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null) return NotFound("Movie not found");

        return Ok(movie.ToDto());
    }

    [HttpGet("image/{fileName}")]
    public IActionResult GetImage(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return BadRequest("Image file name is required");

        var safeFileName = Path.GetFileName(fileName);
        if (!AllowedImageExtensions.Contains(Path.GetExtension(safeFileName)))
            return BadRequest("Invalid image extension");

        var webRootPath = string.IsNullOrWhiteSpace(_environment.WebRootPath)
            ? Path.Combine(_environment.ContentRootPath, "wwwroot")
            : _environment.WebRootPath;
        var filePath = Path.Combine(webRootPath, "images", safeFileName);

        if (!System.IO.File.Exists(filePath))
            return NotFound("Image not found");

        var contentType = Path.GetExtension(safeFileName).ToLowerInvariant() switch
        {
            ".png" => "image/png",
            ".webp" => "image/webp",
            _ => "image/jpeg"
        };

        return PhysicalFile(filePath, contentType);
    }

    [HttpPost("upload-image")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file uploaded" });

        if (!IsAllowedImage(file))
            return BadRequest(new { message = "Only JPG, PNG and WEBP images are allowed" });

        try
        {
            var fileName = await SaveImageToWebRootAsync(file);

            var imageUrl = $"/api/movies/image/{fileName}";
            return Ok(new { url = imageUrl });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error uploading file", error = ex.Message });
        }
    }

    [HttpPost("{movieId}/upload-image")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UploadImageForMovie(int movieId, [FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file uploaded" });

        if (!IsAllowedImage(file))
            return BadRequest(new { message = "Only JPG, PNG and WEBP images are allowed" });

        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == movieId);
        if (movie == null)
            return NotFound(new { message = "Movie not found" });

        try
        {
            var fileName = await SaveImageToWebRootAsync(file);

            var imageUrl = $"/api/movies/image/{fileName}";
            movie.ImagePath = imageUrl;
            await _context.SaveChangesAsync();

            return Ok(new { url = imageUrl, message = "Image uploaded successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error uploading file", error = ex.Message });
        }
    }

    private static bool IsAllowedImage(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrWhiteSpace(extension) || !AllowedImageExtensions.Contains(extension))
            return false;

        if (string.IsNullOrWhiteSpace(file.ContentType))
            return true;

        return AllowedImageContentTypes.Contains(file.ContentType);
    }

    private async Task<string> SaveImageToWebRootAsync(IFormFile file)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var webRootPath = string.IsNullOrWhiteSpace(_environment.WebRootPath)
            ? Path.Combine(_environment.ContentRootPath, "wwwroot")
            : _environment.WebRootPath;
        var uploadPath = Path.Combine(webRootPath, "images");

        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        var filePath = Path.Combine(uploadPath, fileName);
        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return fileName;
    }
}
