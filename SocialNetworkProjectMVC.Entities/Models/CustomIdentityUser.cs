using Microsoft.AspNetCore.Identity;
using SocialNetworkProjectMVC.Entities.Enums;

namespace SocialNetworkProjectMVC.Entities.Models;
public class CustomIdentityUser : IdentityUser
{
    // additional properties here : 
    public string ? ConnectTime { get; set; } = string.Empty;
    public DateTime DisconnectTime { get; set; } = DateTime.Now;
    public string? ImageUrl { get; set; }
    public string? BackgroundImageUrl { get; set; } = "personcover.jpg";
    public bool IsOnline { get; set; }

    // navigation properties : 
    public virtual ICollection<Post> ? Posts { get; set; }
    public virtual ICollection<Friend> ? Friends { get; set; }
    public virtual ICollection<FriendRequest> ? FriendRequests { get; set; }
    public virtual ICollection<Chat> ? Chats { get; set; }

    // collection of received notifications :
    public virtual ICollection<Notification> ReceivedNotifications { get; set; }

    // collection of sent notifications :
    public virtual ICollection<Notification> SentNotifications { get; set; }

    // default constructor for initializing the navigation properties :
    public CustomIdentityUser()
    {
        Posts = new List<Post>();
        Friends = new List<Friend>();
        FriendRequests = new List<FriendRequest>();
        Chats = new List<Chat>();
        ReceivedNotifications = new List<Notification>();
        SentNotifications = new List<Notification>();
    }

    // other methods : 
    public int GetAllUnreadMessageCount()
    {
        return Chats.SelectMany(c => c.Messages)
            .Where(m=>!m.IsRead)
            .Count();
    }

    public int GetAllUnreadNotificationCount()
    {
        return ReceivedNotifications.Count();
    }

    public int GetLikeCountOfAllPosts()
    {
        return Posts.SelectMany(p => p.Likes).Count();
    }

    public int GetFollowersCount()
    {
        return FriendRequests.Where(fr => fr.ReceiverId == Id).Count();
    }

    public int GetFollowingCount()
    {
        return FriendRequests.Where(fr => fr.SenderId == Id).Count();
    }

    public int GetPhotosCount()
    {
        return Posts.Where(p => p.PostType == PostType.Image).Count();
    }
}
