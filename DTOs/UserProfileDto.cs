namespace Watchlist_Tracker.DTOs;

using System.ComponentModel.DataAnnotations;

public record UserProfileDto(
    string Id,
    string FullName,
    string Email,
    int ReviewCount,
    int WatchlistCount,
    List<ReviewDto> RecentReviews
);
