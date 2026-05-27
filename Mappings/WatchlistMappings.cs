namespace Watchlist_Tracker.Mappings;

using Watchlist_Tracker.Models;
using Watchlist_Tracker.DTOs;

public static class WatchlistMappings
{
    public static WatchlistDto ToDto(this Watchlist watchlist) => new(
        watchlist.MovieId,
        watchlist.Movie?.Title ?? "Unknown",
        watchlist.Movie?.ImagePath ?? string.Empty,
        watchlist.DateAdded
    );

    public static List<WatchlistDto> ToDtoList(this IEnumerable<Watchlist> watchlists)
        => watchlists.Select(w => w.ToDto()).ToList();
}
