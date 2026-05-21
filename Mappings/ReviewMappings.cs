namespace Watchlist_Tracker.Mappings;

using Watchlist_Tracker.Models;
using Watchlist_Tracker.DTOs;

public static class ReviewMappings
{
    public static Review ToEntity(this CreateReviewDto dto, string authorId) => new()
    {
        MovieId = dto.MovieId,
        Content = dto.Content,
        Rating = dto.Rating,
        Visibility = dto.Visibility,
        AuthorId = authorId,
        WatchedAt = DateTime.Now
    };

    public static List<ReviewDto> ToDtoList(this IEnumerable<Review> reviews)
        => reviews.Select(a => a.ToDto()).ToList();

    public static ReviewDto ToDto(this Review review) => new(
        review.Id,
        review.Author?.FullName ?? "Unknown",
        review.Movie?.Title ?? "Unknown",
        review.Rating,
        review.Content ?? string.Empty,
        review.WatchedAt,
        review.Comments?.Count ?? 0
    );
}