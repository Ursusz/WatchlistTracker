namespace Watchlist_Tracker.DTOs;

using System.ComponentModel.DataAnnotations;

public record LoginDto(
    [Required, EmailAddress] string Email,
    [Required] string Password
);