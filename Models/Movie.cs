namespace Watchlist_Tracker.Models;

using System.ComponentModel.DataAnnotations;

public class Movie : BaseEntity
{
    [Required]
    [MinLength(2)]
    public string Title { get; set; } = String.Empty;

    public string Description { get; set; } = String.Empty;

    [DataType(DataType.Date)]
    public DateTime PublishedAt { get; set; } = DateTime.Now;

    public int? CategoryId { get; set; }

    public Category? Category { get; set; }

    public string? ImagePath { get; set; }

    public List<Review> Reviews { get; set; } = [];

    public List<Watchlist> Watchlist { get; set; } = [];
}