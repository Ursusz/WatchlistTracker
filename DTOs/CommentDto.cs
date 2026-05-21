namespace Watchlist_Tracker.DTOs;

using System.ComponentModel.DataAnnotations;

public record CommentDto(
    int Id,
    string AuthorName,
    string Text,
    DateTime CreatedAt
);