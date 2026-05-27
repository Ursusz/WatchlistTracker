namespace Watchlist_Tracker.Repositories;

using Microsoft.EntityFrameworkCore;
using Watchlist_Tracker.Data;
using Watchlist_Tracker.Models;

public interface IWatchlistRepository : IRepository<Watchlist>
{
    Task<IEnumerable<Watchlist>> GetUserWatchlistAsync(string userId);
    Task<Watchlist?> GetWatchlistItemAsync(string userId, int movieId);
}

public class WatchlistRepository : Repository<Watchlist>, IWatchlistRepository
{
    private readonly AppDbContext _context;

    public WatchlistRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Watchlist>> GetUserWatchlistAsync(string userId)
        => await _context.Watchlists
            .Where(w => w.UserId == userId)
            .Include(w => w.Movie)
            .OrderByDescending(w => w.DateAdded)
            .ToListAsync();

    public async Task<Watchlist?> GetWatchlistItemAsync(string userId, int movieId)
        => await _context.Watchlists
            .FirstOrDefaultAsync(w => w.UserId == userId && w.MovieId == movieId);
}
