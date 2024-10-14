using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SocialNetworkProjectMVC.Entities.Models;

namespace SocialNetworkProjectMVC.WebUI.Hubs;
public class ZustHub : Hub
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly UserManager<CustomIdentityUser> _userManager;
    public ZustHub(IHttpContextAccessor contextAccessor, UserManager<CustomIdentityUser> userManager)
    {
        _contextAccessor = contextAccessor;
        _userManager = userManager;
    }

    // sending to all users that user connected and is online now :
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"User connected: {Context.ConnectionId}");
        await Clients.All.SendAsync("UpdateContacts");
    }

    // sending to all users that user disconnected and is offline now :
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);
        user.IsOnline = false;
        await _userManager.UpdateAsync(user);
        await Clients.All.SendAsync("UpdateContacts");
    }

    // updating all contacts view - layout for all users :
    public async Task UpdateContactsForAllUsers()
    {
        await Clients.All.SendAsync("UpdateContacts");
    }

    // updating contacts view - layout for others :
    public async Task UpdateContactsForOtherUsers()
    {
        await Clients.Others.SendAsync("UpdateContacts");
    }

    // updating user messages for receiver :
   public async Task UpdateUserMessagesForReceiver(string receiverId,string senderId)
    {
        await Clients.User(receiverId).SendAsync("UpdateAllMessages",senderId);
    }

    // updating notifications for receiver user : 
    public async Task UpdateNotificationsForReceiver(string receiverId)
    {
        await Clients.User(receiverId).SendAsync("UpdateNotificationsForReceiver");
    }

    // updating friend requests for receiver and sender : 
    public async Task UpdateFriendRequestsAndFriendsForUsers()
    {
        await Clients.All.SendAsync("UpdateFriendRequestsAndFriends");
    }
}
