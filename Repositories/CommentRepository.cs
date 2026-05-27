namespace Watchlist_Tracker.Repositories;

using Microsoft.EntityFrameworkCore;
using Watchlist_Tracker.Data;
using Watchlist_Tracker.Models;

public interface ICommentRepository : IRepository<Comment>
{
    Task<IEnumerable<Comment>> GetCommentsByReviewIdAsync(int reviewId);
}

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    private readonly AppDbContext _context;

    public CommentRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Comment>> GetCommentsByReviewIdAsync(int reviewId)
        => await _context.Comments
            .Where(c => c.ReviewId == reviewId)
            .Include(c => c.User)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
}
