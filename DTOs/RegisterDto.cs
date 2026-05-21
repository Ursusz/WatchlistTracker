namespace Watchlist_Tracker.DTOs;

using System.ComponentModel.DataAnnotations;

public record RegisterDto(
    [Required, MinLength(3)] string FullName,
    [Required, EmailAddress] string Email,
    [Required, MinLength(6)] string Password,
    [Required, Compare("Password")] string ConfirmPassword
);