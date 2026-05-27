namespace Watchlist_Tracker.Services;

using Watchlist_Tracker.DTOs;
using Watchlist_Tracker.Models;
using Watchlist_Tracker.Repositories;
using Watchlist_Tracker.Mappings;

public interface IReviewService
{
    Task<ReviewDto?> GetReviewByIdAsync(int id);
    Task<IEnumerable<ReviewDto>> GetReviewsByMovieAsync(int movieId);
    Task<IEnumerable<ReviewDto>> GetUserReviewsAsync(string userId);
    Task<IEnumerable<ReviewDto>> GetGlobalFeedAsync(string userId);
    Task<IEnumerable<ReviewDto>> GetFriendsFeedAsync(string userId);
    Task<int> CreateReviewAsync(CreateReviewDto dto, string userId);
    Task UpdateReviewAsync(int id, UpdateReviewDto dto, string userId);
    Task DeleteReviewAsync(int id, string userId);
}

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<ReviewDto?> GetReviewByIdAsync(int id)
    {
        var review = await _reviewRepository.GetByIdAsync(id);
        return review?.ToDto();
    }

    public async Task<IEnumerable<ReviewDto>> GetReviewsByMovieAsync(int movieId)
    {
        var reviews = await _reviewRepository.GetReviewsByMovieIdAsync(movieId);
        return reviews.ToDtoList();
    }

    public async Task<IEnumerable<ReviewDto>> GetUserReviewsAsync(string userId)
    {
        var reviews = await _reviewRepository.GetReviewsByUserIdAsync(userId);
        return reviews.ToDtoList();
    }

    public async Task<IEnumerable<ReviewDto>> GetGlobalFeedAsync(string userId)
    {
        var reviews = await _reviewRepository.GetGlobalFeedAsync(userId);
        return reviews.ToDtoList();
    }

    public async Task<IEnumerable<ReviewDto>> GetFriendsFeedAsync(string userId)
    {
        var reviews = await _reviewRepository.GetFriendsFeedAsync(userId);
        return reviews.ToDtoList();
    }

    public async Task<int> CreateReviewAsync(CreateReviewDto dto, string userId)
    {
        var review = dto.ToEntity(userId);
        await _reviewRepository.AddAsync(review);
        return review.Id;
    }

    public async Task UpdateReviewAsync(int id, UpdateReviewDto dto, string userId)
    {
        var review = await _reviewRepository.GetByIdAsync(id);
        if (review == null) throw new InvalidOperationException("Review not found");
        if (review.AuthorId != userId) throw new UnauthorizedAccessException("You can only edit your own reviews");

        review.Rating = dto.Rating;
        review.Content = dto.Content;
        review.Visibility = dto.Visibility;

        _reviewRepository.Update(review);
        await _reviewRepository.SaveChangesAsync();
    }

    public async Task DeleteReviewAsync(int id, string userId)
    {
        var review = await _reviewRepository.GetByIdAsync(id);
        if (review == null) throw new InvalidOperationException("Review not found");
        if (review.AuthorId != userId) throw new UnauthorizedAccessException("You can only delete your own reviews");

        _reviewRepository.Remove(review);
        await _reviewRepository.SaveChangesAsync();
    }
}
