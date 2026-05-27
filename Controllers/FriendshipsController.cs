namespace Watchlist_Tracker.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Watchlist_Tracker.DTOs;
using Watchlist_Tracker.Services;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FriendshipsController : ControllerBase
{
    private readonly IFriendshipService _friendshipService;

    public FriendshipsController(IFriendshipService friendshipService)
    {
        _friendshipService = friendshipService;
    }

    private string GetCurrentUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
               throw new UnauthorizedAccessException("User not authenticated");
    }

    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<FriendshipDto>>> GetPendingRequests()
    {
        var userId = GetCurrentUserId();
        var requests = await _friendshipService.GetPendingRequestsAsync(userId);
        return Ok(requests);
    }

    [HttpGet("pending/count")]
    public async Task<ActionResult<int>> GetPendingRequestCount()
    {
        var userId = GetCurrentUserId();
        var count = await _friendshipService.GetPendingRequestCountAsync(userId);
        return Ok(count);
    }

    [HttpPost("request/{receiverId}")]
    public async Task<ActionResult<int>> SendFriendRequest(string receiverId)
    {
        try
        {
            var senderId = GetCurrentUserId();
            var friendshipId = await _friendshipService.SendFriendRequestAsync(senderId, receiverId);
            return CreatedAtAction(nameof(GetPendingRequests), friendshipId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("request/{friendshipId}/accept")]
    public async Task<IActionResult> AcceptFriendRequest(int friendshipId)
    {
        try
        {
            var userId = GetCurrentUserId();
            await _friendshipService.AcceptFriendRequestAsync(friendshipId, userId);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("request/{friendshipId}")]
    public async Task<IActionResult> RejectFriendRequest(int friendshipId)
    {
        try
        {
            var userId = GetCurrentUserId();
            await _friendshipService.RejectFriendRequestAsync(friendshipId, userId);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
