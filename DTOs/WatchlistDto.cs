namespace Watchlist_Tracker.DTOs;

public record WatchlistDto(
    int MovieId,
    string MovieTitle,
    string MovieImage,
    DateTime DateAdded
);
