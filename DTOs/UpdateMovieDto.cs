namespace Watchlist_Tracker.DTOs;

using System.ComponentModel.DataAnnotations;
public record UpdateMovieDto
(
    [Required, MinLength(5)] string Title,
    [Required, MinLength(10)] string Description,
    [Required] DateTime PublishedAt,
    [Required] int CategoryId,
    IFormFile? Image
);