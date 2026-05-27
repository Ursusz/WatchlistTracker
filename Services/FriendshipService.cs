namespace Watchlist_Tracker.Services;

using Watchlist_Tracker.DTOs;
using Watchlist_Tracker.Models;
using Watchlist_Tracker.Repositories;
using Watchlist_Tracker.Mappings;

public interface IFriendshipService
{
    Task<IEnumerable<FriendshipDto>> GetPendingRequestsAsync(string userId);
    Task<int> SendFriendRequestAsync(string senderId, string receiverId);
    Task AcceptFriendRequestAsync(int friendshipId, string userId);
    Task RejectFriendRequestAsync(int friendshipId, string userId);
    Task<int> GetPendingRequestCountAsync(string userId);
}

public class FriendshipService : IFriendshipService
{
    private readonly IFriendshipRepository _friendshipRepository;

    public FriendshipService(IFriendshipRepository friendshipRepository)
    {
        _friendshipRepository = friendshipRepository;
    }

    public async Task<IEnumerable<FriendshipDto>> GetPendingRequestsAsync(string userId)
    {
        var requests = await _friendshipRepository.GetFriendRequestsAsync(userId);
        return requests.Select(f => f.ToDto()).ToList();
    }

    public async Task<int> SendFriendRequestAsync(string senderId, string receiverId)
    {
        if (senderId == receiverId) throw new InvalidOperationException("Cannot send friend request to yourself");

        var existingFriendship = await _friendshipRepository.GetFriendshipAsync(senderId, receiverId);
        if (existingFriendship != null) throw new InvalidOperationException("Friendship already exists");

        var friendship = new Friendship
        {
            RequesterId = senderId,
            ReceiverId = receiverId,
            Status = FriendshipStatus.Pending
        };

        await _friendshipRepository.AddAsync(friendship);
        return friendship.Id;
    }

    public async Task AcceptFriendRequestAsync(int friendshipId, string userId)
    {
        var friendship = await _friendshipRepository.GetByIdAsync(friendshipId);
        if (friendship == null) throw new InvalidOperationException("Friendship not found");
        if (friendship.ReceiverId != userId) throw new UnauthorizedAccessException("You can only accept requests sent to you");

        friendship.Status = FriendshipStatus.Accepted;
        _friendshipRepository.Update(friendship);
        await _friendshipRepository.SaveChangesAsync();
    }

    public async Task RejectFriendRequestAsync(int friendshipId, string userId)
    {
        var friendship = await _friendshipRepository.GetByIdAsync(friendshipId);
        if (friendship == null) throw new InvalidOperationException("Friendship not found");
        if (friendship.ReceiverId != userId) throw new UnauthorizedAccessException("You can only reject requests sent to you");

        _friendshipRepository.Remove(friendship);
        await _friendshipRepository.SaveChangesAsync();
    }

    public async Task<int> GetPendingRequestCountAsync(string userId)
    {
        var requests = await _friendshipRepository.GetFriendRequestsAsync(userId);
        return requests.Count();
    }
}
