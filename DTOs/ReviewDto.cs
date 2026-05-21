namespace Watchlist_Tracker.DTOs;

using System.ComponentModel.DataAnnotations;

public record ReviewDto(
    int Id,
    string AuthorName,
    string MovieTitle,
    int Rating,
    string Content,
    DateTime WatchedAt,
    int CommentCount
);