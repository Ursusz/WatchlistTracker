namespace Watchlist_Tracker.Controllers;

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

    public MoviesController(AppDbContext context)
    {
        _context = context;
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

    [HttpPost("upload-image")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file uploaded" });

        if (file.ContentType != "image/jpeg" && file.ContentType != "image/png")
            return BadRequest(new { message = "Only JPEG and PNG images are allowed" });

        try
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var uploadPath = Path.Combine("wwwroot", "images");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { url = $"/images/{fileName}" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error uploading file", error = ex.Message });
        }
    }

    [HttpPost("{movieId}/upload-image")]
    public async Task<IActionResult> UploadImageForMovie(int movieId, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file uploaded" });

        if (file.ContentType != "image/jpeg" && file.ContentType != "image/png")
            return BadRequest(new { message = "Only JPEG and PNG images are allowed" });

        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == movieId);
        if (movie == null)
            return NotFound(new { message = "Movie not found" });

        try
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var uploadPath = Path.Combine("wwwroot", "images");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageUrl = $"/images/{fileName}";
            movie.ImagePath = imageUrl;
            await _context.SaveChangesAsync();

            return Ok(new { url = imageUrl, message = "Image uploaded successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error uploading file", error = ex.Message });
        }
    }
}
