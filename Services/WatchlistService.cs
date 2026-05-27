namespace Watchlist_Tracker.Services;

using Watchlist_Tracker.DTOs;
using Watchlist_Tracker.Models;
using Watchlist_Tracker.Repositories;
using Watchlist_Tracker.Mappings;

public interface IWatchlistService
{
    Task<IEnumerable<WatchlistDto>> GetUserWatchlistAsync(string userId);
    Task AddToWatchlistAsync(string userId, int movieId);
    Task RemoveFromWatchlistAsync(string userId, int movieId);
    Task<bool> IsInWatchlistAsync(string userId, int movieId);
}

public class WatchlistService : IWatchlistService
{
    private readonly IWatchlistRepository _watchlistRepository;

    public WatchlistService(IWatchlistRepository watchlistRepository)
    {
        _watchlistRepository = watchlistRepository;
    }

    public async Task<IEnumerable<WatchlistDto>> GetUserWatchlistAsync(string userId)
    {
        var items = await _watchlistRepository.GetUserWatchlistAsync(userId);
        return items.Select(w => w.ToDto()).ToList();
    }

    public async Task AddToWatchlistAsync(string userId, int movieId)
    {
        var existing = await _watchlistRepository.GetWatchlistItemAsync(userId, movieId);
        if (existing != null) throw new InvalidOperationException("Movie already in watchlist");

        var watchlistItem = new Watchlist
        {
            UserId = userId,
            MovieId = movieId,
            DateAdded = DateTime.Now
        };

        await _watchlistRepository.AddAsync(watchlistItem);
    }

    public async Task RemoveFromWatchlistAsync(string userId, int movieId)
    {
        var item = await _watchlistRepository.GetWatchlistItemAsync(userId, movieId);
        if (item == null) throw new InvalidOperationException("Movie not in watchlist");

        _watchlistRepository.Remove(item);
        await _watchlistRepository.SaveChangesAsync();
    }

    public async Task<bool> IsInWatchlistAsync(string userId, int movieId)
    {
        var item = await _watchlistRepository.GetWatchlistItemAsync(userId, movieId);
        return item != null;
    }
}
