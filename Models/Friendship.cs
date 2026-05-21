namespace Watchlist_Tracker.Models;

using System.ComponentModel.DataAnnotations;

public enum FriendshipStatus { Pending, Accepted, Blocked }

public class Friendship : BaseEntity
{
    public string? RequesterId { get; set; } = string.Empty;
    public ApplicationUser? Requester { get; set; }
    public string? ReceiverId { get; set; } = string.Empty;
    public ApplicationUser? Receiver { get; set; }
    public FriendshipStatus Status { get; set; }
}