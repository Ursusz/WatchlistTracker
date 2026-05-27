namespace Watchlist_Tracker.Services;

using Watchlist_Tracker.DTOs;
using Watchlist_Tracker.Models;
using Watchlist_Tracker.Repositories;
using Watchlist_Tracker.Mappings;

public interface ICommentService
{
    Task<IEnumerable<CommentDto>> GetCommentsByReviewAsync(int reviewId);
    Task<int> CreateCommentAsync(CreateCommentDto dto, string userId);
    Task DeleteCommentAsync(int id, string userId, bool isAdmin);
}

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IReviewRepository _reviewRepository;

    public CommentService(ICommentRepository commentRepository, IReviewRepository reviewRepository)
    {
        _commentRepository = commentRepository;
        _reviewRepository = reviewRepository;
    }

    public async Task<IEnumerable<CommentDto>> GetCommentsByReviewAsync(int reviewId)
    {
        var comments = await _commentRepository.GetCommentsByReviewIdAsync(reviewId);
        return comments.Select(c => c.ToDto()).ToList();
    }

    public async Task<int> CreateCommentAsync(CreateCommentDto dto, string userId)
    {
        var review = await _reviewRepository.GetByIdAsync(dto.ReviewId);
        if (review == null) throw new InvalidOperationException("Review not found");

        var comment = new Comment
        {
            ReviewId = dto.ReviewId,
            UserId = userId,
            Text = dto.Text,
            CreatedAt = DateTime.Now
        };

        await _commentRepository.AddAsync(comment);
        return comment.Id;
    }

    public async Task DeleteCommentAsync(int id, string userId, bool isAdmin)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment == null) throw new InvalidOperationException("Comment not found");
        if (!isAdmin && comment.UserId != userId)
            throw new UnauthorizedAccessException("You can only delete your own comments");

        _commentRepository.Remove(comment);
        await _commentRepository.SaveChangesAsync();
    }
}
