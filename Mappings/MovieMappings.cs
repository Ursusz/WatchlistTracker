namespace Watchlist_Tracker.Mappings;

using Watchlist_Tracker.Models;
using Watchlist_Tracker.DTOs;

public static class MovieMappings
{
    public static MovieDto ToDto(this Movie movie) => new(
        Id: movie.Id,
        Title: movie.Title,
        Description: movie.Description,
        PublishedAt: movie.PublishedAt,
        CategoryName: movie.Category?.Name,
        ImagePath: movie.ImagePath,
        AverageRating: movie.Reviews.Any() ? movie.Reviews.Average(r => r.Rating) : 0.0,
        ReviewCount: movie.Reviews.Count
    );

    public static List<MovieDto> ToDtoList(this IEnumerable<Movie> movies)
        => movies.Select(a => a.ToDto()).ToList();

    public static Movie ToEntity(this CreateMovieDto dto, string? imagePath) => new()
    {
        Title = dto.Title,
        Description = dto.Description,
        CategoryId = dto.CategoryId,
        PublishedAt = dto.PublishedAt,
        ImagePath = imagePath
    };

    public static void ApplyTo(this UpdateMovieDto dto, Movie movie, string? newImagePath = null)
    {
        movie.Title = dto.Title;
        movie.Description = dto.Description;
        movie.CategoryId = dto.CategoryId;
        movie.PublishedAt = dto.PublishedAt;

        if (!string.IsNullOrEmpty(newImagePath))
        {
            movie.ImagePath = newImagePath;
        }
    }
}