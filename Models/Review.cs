namespace Watchlist_Tracker.Models;

using System.ComponentModel.DataAnnotations;

public enum Visibility { Public, FriendsOnly }

public class Review : BaseEntity
{
    [Required]
    public string AuthorId { get; set; } = String.Empty;
    public ApplicationUser? Author { get; set; }

    [Required]
    public int MovieId { get; set; }
    public Movie? Movie { get; set; }

    public int Rating { get; set; }

    [MinLength(2)]
    public string? Content { get; set; }

    [DataType(DataType.Date)]
    public DateTime WatchedAt { get; set; } = DateTime.Now;

    public Visibility Visibility { get; set; }

    public List<Comment> Comments { get; set; } = [];

}