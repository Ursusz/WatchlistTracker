namespace Watchlist_Tracker.DTOs;

using System.ComponentModel.DataAnnotations;
using Watchlist_Tracker.Models;

public record UpdateReviewDto(
    [Required, Range(1, 10)] int Rating,
    [Required, MinLength(10)] string Content,
    [Required] Visibility Visibility
);
