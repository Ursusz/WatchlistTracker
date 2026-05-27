namespace Watchlist_Tracker.Mappings;

using Watchlist_Tracker.Models;
using Watchlist_Tracker.DTOs;

public static class FriendshipMappings
{
    public static FriendshipDto ToDto(this Friendship friendship) => new(
        friendship.Id,
        friendship.Requester?.FullName ?? "Unknown",
        friendship.Status.ToString()
    );

    public static List<FriendshipDto> ToDtoList(this IEnumerable<Friendship> friendships)
        => friendships.Select(f => f.ToDto()).ToList();
}
