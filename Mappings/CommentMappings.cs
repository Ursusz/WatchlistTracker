namespace Watchlist_Tracker.Mappings;

using Watchlist_Tracker.Models;
using Watchlist_Tracker.DTOs;
public static class CommentMappings
{
    public static Comment ToEntity(this CreateCommentDto dto, string userId) => new()
    {
        ReviewId = dto.ReviewId,
        Text = dto.Text,
        UserId = userId,
        CreatedAt = DateTime.Now
    };
}
