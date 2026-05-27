namespace Watchlist_Tracker.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Watchlist_Tracker.DTOs;
using Watchlist_Tracker.Services;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WatchlistController : ControllerBase
{
    private readonly IWatchlistService _watchlistService;

    public WatchlistController(IWatchlistService watchlistService)
    {
        _watchlistService = watchlistService;
    }

    private string GetCurrentUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
               throw new UnauthorizedAccessException("User not authenticated");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WatchlistDto>>> GetUserWatchlist()
    {
        var userId = GetCurrentUserId();
        var watchlist = await _watchlistService.GetUserWatchlistAsync(userId);
        return Ok(watchlist);
    }

    [HttpPost("{movieId}")]
    public async Task<IActionResult> AddToWatchlist(int movieId)
    {
        try
        {
            var userId = GetCurrentUserId();
            await _watchlistService.AddToWatchlistAsync(userId, movieId);
            return CreatedAtAction(nameof(GetUserWatchlist), null);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{movieId}")]
    public async Task<IActionResult> RemoveFromWatchlist(int movieId)
    {
        try
        {
            var userId = GetCurrentUserId();
            await _watchlistService.RemoveFromWatchlistAsync(userId, movieId);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("check/{movieId}")]
    public async Task<ActionResult<bool>> IsInWatchlist(int movieId)
    {
        var userId = GetCurrentUserId();
        var isInWatchlist = await _watchlistService.IsInWatchlistAsync(userId, movieId);
        return Ok(isInWatchlist);
    }
}
