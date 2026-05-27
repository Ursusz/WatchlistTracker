namespace Watchlist_Tracker.Repositories;

using Microsoft.EntityFrameworkCore;
using Watchlist_Tracker.Data;
using Watchlist_Tracker.Models;

public interface IReviewRepository : IRepository<Review>
{
    Task<IEnumerable<Review>> GetReviewsByMovieIdAsync(int movieId);
    Task<IEnumerable<Review>> GetReviewsByUserIdAsync(string userId);
    Task<IEnumerable<Review>> GetPublicReviewsAsync();
    Task<IEnumerable<Review>> GetGlobalFeedAsync(string userId);
    Task<IEnumerable<Review>> GetFriendsFeedAsync(string userId);
}

public class ReviewRepository : Repository<Review>, IReviewRepository
{
    private readonly AppDbContext _context;

    public ReviewRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Review>> GetReviewsByMovieIdAsync(int movieId)
        => await _context.Reviews
            .Where(r => r.MovieId == movieId && r.Visibility == Visibility.Public)
            .Include(r => r.Author)
            .Include(r => r.Movie)
            .Include(r => r.Comments)
            .OrderByDescending(r => r.WatchedAt)
            .ToListAsync();

    public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(string userId)
        => await _context.Reviews
            .Where(r => r.AuthorId == userId)
            .Include(r => r.Author)
            .Include(r => r.Movie)
            .Include(r => r.Comments)
            .OrderByDescending(r => r.WatchedAt)
            .ToListAsync();

    public async Task<IEnumerable<Review>> GetPublicReviewsAsync()
        => await _context.Reviews
            .Where(r => r.Visibility == Visibility.Public)
            .Include(r => r.Author)
            .Include(r => r.Movie)
            .Include(r => r.Comments)
            .OrderByDescending(r => r.WatchedAt)
            .ToListAsync();

    public async Task<IEnumerable<Review>> GetGlobalFeedAsync(string userId)
    {
        // Get current user's friends
        var friendIds = await _context.Friendships
            .Where(f => (f.RequesterId == userId || f.ReceiverId == userId) && f.Status == FriendshipStatus.Accepted)
            .Select(f => f.RequesterId == userId ? f.ReceiverId : f.RequesterId)
            .ToListAsync();

        // Get all public reviews + friends-only reviews from friends
        var reviews = await _context.Reviews
            .Where(r => 
                r.Visibility == Visibility.Public ||
                (r.Visibility == Visibility.FriendsOnly && friendIds.Contains(r.AuthorId)) ||
                r.AuthorId == userId
            )
            .Include(r => r.Author)
            .Include(r => r.Movie)
            .Include(r => r.Comments)
            .ToListAsync();

        // Order with friends' reviews first
        return reviews.OrderByDescending(r => friendIds.Contains(r.AuthorId) ? 1 : 0)
            .ThenByDescending(r => r.WatchedAt);
    }

    public async Task<IEnumerable<Review>> GetFriendsFeedAsync(string userId)
    {
        // Get current user's accepted friendships
        var friendIds = await _context.Friendships
            .Where(f => (f.RequesterId == userId || f.ReceiverId == userId) && f.Status == FriendshipStatus.Accepted)
            .Select(f => f.RequesterId == userId ? f.ReceiverId : f.RequesterId)
            .ToListAsync();

        // Get reviews only from friends
        return await _context.Reviews
            .Where(r => friendIds.Contains(r.AuthorId))
            .Include(r => r.Author)
            .Include(r => r.Movie)
            .Include(r => r.Comments)
            .OrderByDescending(r => r.WatchedAt)
            .ToListAsync();
    }
}
