namespace Watchlist_Tracker.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Watchlist_Tracker.DTOs;
using Watchlist_Tracker.Services;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    private string GetCurrentUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
               throw new UnauthorizedAccessException("User not authenticated");
    }

    [HttpGet("review/{reviewId}")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsByReview(int reviewId)
    {
        var comments = await _commentService.GetCommentsByReviewAsync(reviewId);
        return Ok(comments);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<int>> CreateComment(CreateCommentDto dto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var commentId = await _commentService.CreateCommentAsync(dto, userId);
            return CreatedAtAction(nameof(GetCommentsByReview), new { reviewId = dto.ReviewId }, commentId);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteComment(int id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var isAdmin = User.IsInRole("Admin");
            await _commentService.DeleteCommentAsync(id, userId, isAdmin);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid("You can only delete your own comments unless you are an admin");
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
