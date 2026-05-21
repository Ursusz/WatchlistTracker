namespace Watchlist_Tracker.DTOs;

public record MovieDto(
    int Id,
    string Title,
    string Description,
    DateTime PublishedAt,
    string? CategoryName,
    string? ImagePath,
    double AverageRating,
    int ReviewCount);