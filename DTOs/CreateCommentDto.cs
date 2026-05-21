namespace Watchlist_Tracker.DTOs;

using System.ComponentModel.DataAnnotations;

public record CreateCommentDto(
    [Required] int ReviewId, 
    [Required] string Text
);