using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SocialNetworkProjectMVC.Business.Abstract;
using SocialNetworkProjectMVC.DataAccess.Abstract;
using SocialNetworkProjectMVC.Entities.Models;

namespace SocialNetworkProjectMVC.Business.Concrete;
public class FriendService : IFriendService
{
    // private readonly fields for injecting :
    private readonly IFriendDal _friendDal;
    private readonly IHttpContextAccessor _context;
    private readonly UserManager<CustomIdentityUser> _userManager;
    private readonly IFriendRequestDal _friendRequestDal;

    // parametric constructor for injecting : 
    public FriendService(IFriendDal friendDal, IHttpContextAccessor context, UserManager<CustomIdentityUser> userManager, IFriendRequestDal friendRequestDal)
    {
        _friendDal = friendDal;
        _context = context;
        _userManager = userManager;
        _friendRequestDal = friendRequestDal;
    }

    public async Task AddFriendAsync(string friendId)
    {
        var currentUser = await _userManager.GetUserAsync(_context.HttpContext.User);
        var newFriend = new Friend
        {
            OwnId = currentUser.Id,
            YourFriendId = friendId,
        };
        await _friendDal.AddAsync(newFriend);
    }

    public async Task<List<Friend>> GetFriendsOfCurrentUserAsync(string key = "")
    {
        var currentUser = await _userManager.GetUserAsync(_context.HttpContext.User);
        var friends = await _friendDal.GetListAsync();
        if(key != "") return friends.Where(f => f.YourFriendId == currentUser.Id || f.OwnId == currentUser.Id && f.YourFriend.UserName.Contains(key) || f.Own.UserName.Contains(key)).ToList();
        return friends.Where(f => f.YourFriendId == currentUser.Id || f.OwnId == currentUser.Id).ToList();
    }

    public async Task RemoveFriendAsync(string friendId)
    {
        var currentUser = await _userManager.GetUserAsync(_context.HttpContext.User);
        var friends = await _friendDal.GetListAsync();
        var friendRow = friends.FirstOrDefault(f => f.YourFriendId == friendId && f.OwnId == currentUser.Id || f.OwnId == friendId && f.YourFriendId == currentUser.Id);

        var friendRequests = await _friendRequestDal.GetListAsync();
        var friendRequestRow = friendRequests.FirstOrDefault(fr => fr.SenderId == friendId && fr.ReceiverId == currentUser.Id || fr.ReceiverId == friendId && fr.SenderId == currentUser.Id);
        var friendRequest2Row = friendRequests.FirstOrDefault(fr => fr.SenderId == friendId && fr.ReceiverId == currentUser.Id || fr.ReceiverId == friendId && fr.SenderId == currentUser.Id);

        await _friendDal.DeleteAsync(friendRow);

        await _friendRequestDal.DeleteAsync(friendRequestRow);
        await _friendRequestDal.DeleteAsync(friendRequest2Row);
    }

    public async Task<List<CustomIdentityUser>> GetOtherPeopleAsync(string key = "")
    {
        // Get the current user
        var currentUser = await _userManager.GetUserAsync(_context.HttpContext.User);

        // Fetch all users
        var users = await _userManager.Users.ToListAsync();

        // Get list of current user's friends and friend requests (sent and received)
        var currentUserFriends = currentUser.Friends.Select(f => f.YourFriendId).ToList();
        var currentUserFriends2 = currentUser.Friends.Select(f => f.OwnId).ToList();

        var allRequests = await _friendRequestDal.GetListAsync();
        var sentFriendRequests = allRequests.Where(fr => fr.SenderId == currentUser.Id).Select(fr => fr.ReceiverId).ToList();
        var receivedFriendRequests = allRequests.Where(fr => fr.ReceiverId == currentUser.Id).Select(fr => fr.SenderId).ToList();

        // Filter users who are not:
        // 1. Friends
        // 2. Sent a friend request by the current user
        // 3. Received a friend request from the current user
        var otherUsers = users.Where(u =>
            u.Id != currentUser.Id &&  // Exclude current user
            !currentUserFriends.Contains(u.Id) && !currentUserFriends2.Contains(u.Id) &&  // Exclude friends
            !sentFriendRequests.Contains(u.Id) &&  // Exclude users who received a request from current user
            !receivedFriendRequests.Contains(u.Id) // Exclude users who sent a request to current user
        );

        // If a search key is provided, filter by username
        if (!string.IsNullOrEmpty(key))
        {
            otherUsers = otherUsers.Where(u => u.UserName.Contains(key));
        }

        return otherUsers.ToList();
    }
}
