namespace Watchlist_Tracker.Repositories;

using Microsoft.EntityFrameworkCore;
using Watchlist_Tracker.Data;
using Watchlist_Tracker.Models;

public interface IUserRepository : IRepository<ApplicationUser>
{
    Task<ApplicationUser?> GetUserByIdWithReviewsAsync(string userId);
    Task<ApplicationUser?> GetUserByIdWithWatchlistAsync(string userId);
}

public class UserRepository : Repository<ApplicationUser>, IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ApplicationUser?> GetUserByIdWithReviewsAsync(string userId)
        => await _context.Users
            .Include(u => u.Reviews)
            .Include(u => u.Comments)
            .FirstOrDefaultAsync(u => u.Id == userId);

    public async Task<ApplicationUser?> GetUserByIdWithWatchlistAsync(string userId)
        => await _context.Users
            .Include(u => u.Watchlist)
            .ThenInclude(w => w.Movie)
            .FirstOrDefaultAsync(u => u.Id == userId);
}
