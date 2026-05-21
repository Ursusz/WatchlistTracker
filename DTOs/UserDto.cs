namespace Watchlist_Tracker.DTOs;

using System.ComponentModel.DataAnnotations;

public record UserDto(
    string Id,
    string FullName,
    string Email
);