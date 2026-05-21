namespace Watchlist_Tracker.Mappings;

using Watchlist_Tracker.Models;
using Watchlist_Tracker.DTOs;
public static class ApplicationUserMapping
{
    public static UserDto ToDto(this ApplicationUser user) => new(
        Id: user.Id,
        FullName: user.FullName,
        Email: user.Email ?? string.Empty
    );
}
