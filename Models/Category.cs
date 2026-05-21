namespace Watchlist_Tracker.Models;

using System.ComponentModel.DataAnnotations;
public class Category : BaseEntity
{
    [Required]
    [MinLength(2)]
    public string Name { get; set; } = string.Empty;

    public List<Movie> Movies { get; set; } = [];
}