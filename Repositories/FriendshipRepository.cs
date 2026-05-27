namespace Watchlist_Tracker.Repositories;

using Microsoft.EntityFrameworkCore;
using Watchlist_Tracker.Data;
using Watchlist_Tracker.Models;

public interface IFriendshipRepository : IRepository<Friendship>
{
    Task<IEnumerable<Friendship>> GetFriendRequestsAsync(string userId);
    Task<IEnumerable<Friendship>> GetAcceptedFriendsAsync(string userId);
    Task<Friendship?> GetFriendshipAsync(string userId1, string userId2);
}

public class FriendshipRepository : Repository<Friendship>, IFriendshipRepository
{
    private readonly AppDbContext _context;

    public FriendshipRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Friendship>> GetFriendRequestsAsync(string userId)
        => await _context.Friendships
            .Where(f => f.ReceiverId == userId && f.Status == FriendshipStatus.Pending)
            .Include(f => f.Requester)
            .ToListAsync();

    public async Task<IEnumerable<Friendship>> GetAcceptedFriendsAsync(string userId)
        => await _context.Friendships
            .Where(f => (f.RequesterId == userId || f.ReceiverId == userId) && f.Status == FriendshipStatus.Accepted)
            .Include(f => f.Requester)
            .Include(f => f.Receiver)
            .ToListAsync();

    public async Task<Friendship?> GetFriendshipAsync(string userId1, string userId2)
        => await _context.Friendships
            .FirstOrDefaultAsync(f =>
                (f.RequesterId == userId1 && f.ReceiverId == userId2) ||
                (f.RequesterId == userId2 && f.ReceiverId == userId1));
}
