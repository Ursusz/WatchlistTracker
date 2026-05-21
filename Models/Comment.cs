namespace Watchlist_Tracker.Models;

using System.ComponentModel.DataAnnotations;

public class Comment: BaseEntity
{
    public int ReviewId { get; set; }
    public Review? Review { get; set; }
    public string UserId { get; set; } = String.Empty;
    public ApplicationUser? User { get; set; }

    public string Text { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}