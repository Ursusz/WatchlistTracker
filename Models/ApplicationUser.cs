namespace Watchlist_Tracker.Models;

using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    public List<Review> Reviews { get; set; } = [];
    public List<Comment> Comments { get; set; } = [];

    public List<Friendship> SentFriendships { get; set; } = [];

    public List<Friendship> ReceivedFriendships { get; set; } = [];
}