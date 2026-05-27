namespace Watchlist_Tracker.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Watchlist_Tracker.DTOs;
using Watchlist_Tracker.Repositories;
using Watchlist_Tracker.Mappings;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IReviewRepository _reviewRepository;

    public UsersController(IUserRepository userRepository, IReviewRepository reviewRepository)
    {
        _userRepository = userRepository;
        _reviewRepository = reviewRepository;
    }

    private string GetCurrentUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
               throw new UnauthorizedAccessException("User not authenticated");
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<UserProfileDto>> GetUserProfile(string userId)
    {
        var user = await _userRepository.GetUserByIdWithReviewsAsync(userId);
        if (user == null) return NotFound("User not found");

        var reviews = await _reviewRepository.GetReviewsByUserIdAsync(userId);
        var recentReviews = reviews.Take(5).ToDtoList();

        var userWithWatchlist = await _userRepository.GetUserByIdWithWatchlistAsync(userId);
        var watchlistCount = userWithWatchlist?.Watchlist?.Count ?? 0;

        var profile = new UserProfileDto(
            Id: user.Id,
            FullName: user.FullName,
            Email: user.Email ?? string.Empty,
            ReviewCount: reviews.Count(),
            WatchlistCount: watchlistCount,
            RecentReviews: recentReviews
        );

        return Ok(profile);
    }

    [HttpGet("current/profile")]
    [Authorize]
    public async Task<ActionResult<UserProfileDto>> GetCurrentUserProfile()
    {
        var userId = GetCurrentUserId();
        return await GetUserProfile(userId);
    }
}
