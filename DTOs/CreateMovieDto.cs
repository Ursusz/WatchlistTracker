namespace Watchlist_Tracker.DTOs;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

public record CreateMovieDto(
    [Required, MinLength(2)] string Title,
    [Required, MinLength(10)] string Description,
    [Required] DateTime PublishedAt,
    [Required] int CategoryId,
    IFormFile? ImageFile
);