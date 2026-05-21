namespace Watchlist_Tracker.DTOs;

using System.ComponentModel.DataAnnotations;

public record FriendshipDto(
    int FriendshipId, 
    string RequesterName, 
    string Status
);