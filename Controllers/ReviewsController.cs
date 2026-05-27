namespace Watchlist_Tracker.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Watchlist_Tracker.DTOs;
using Watchlist_Tracker.Services;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    private string GetCurrentUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
               throw new UnauthorizedAccessException("User not authenticated");
    }

    [HttpGet("movie/{movieId}")]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsByMovie(int movieId)
    {
        var reviews = await _reviewService.GetReviewsByMovieAsync(movieId);
        return Ok(reviews);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetUserReviews(string userId)
    {
        var reviews = await _reviewService.GetUserReviewsAsync(userId);
        return Ok(reviews);
    }

    [HttpGet("feed/global")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetGlobalFeed()
    {
        var userId = GetCurrentUserId();
        var reviews = await _reviewService.GetGlobalFeedAsync(userId);
        return Ok(reviews);
    }

    [HttpGet("feed/friends")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetFriendsFeed()
    {
        var userId = GetCurrentUserId();
        var reviews = await _reviewService.GetFriendsFeedAsync(userId);
        return Ok(reviews);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReviewDto>> GetReviewById(int id)
    {
        var review = await _reviewService.GetReviewByIdAsync(id);
        if (review == null) return NotFound("Review not found");
        return Ok(review);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<int>> CreateReview(CreateReviewDto dto)
    {
        var userId = GetCurrentUserId();
        var reviewId = await _reviewService.CreateReviewAsync(dto, userId);
        return CreatedAtAction(nameof(GetReviewById), new { id = reviewId }, reviewId);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateReview(int id, UpdateReviewDto dto)
    {
        try
        {
            var userId = GetCurrentUserId();
            await _reviewService.UpdateReviewAsync(id, dto, userId);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid("You can only edit your own reviews");
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteReview(int id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var isAdmin = User.IsInRole("Admin");
            await _reviewService.DeleteReviewAsync(id, userId, isAdmin);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid("You can only delete your own reviews unless you are an admin");
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
