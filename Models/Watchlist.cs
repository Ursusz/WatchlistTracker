namespace Watchlist_Tracker.Models;

using System.ComponentModel.DataAnnotations;

public class Watchlist : BaseEntity
{
    [Required]
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }

    [Required]
    public int MovieId { get; set; }
    public Movie? Movie { get; set; }

    [DataType(DataType.Date)]
    public DateTime DateAdded { get; set; } = DateTime.Now;
}
