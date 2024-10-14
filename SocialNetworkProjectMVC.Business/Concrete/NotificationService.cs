using SocialNetworkProjectMVC.Business.Abstract;
using SocialNetworkProjectMVC.DataAccess.Abstract;
using SocialNetworkProjectMVC.Entities.Models;

namespace SocialNetworkProjectMVC.Business.Concrete;
public class NotificationService : INotificationService
{
    // private readonly fields for injection : 
    private readonly INotificationDal _notificationDal;

    // parametric constructor for injecting :
    public NotificationService(INotificationDal notificationDal)
    {
        _notificationDal = notificationDal;
    }

    // implemented methods : 
    public async Task AddNotificationAsync(string senderId, string receiverId,string notificationText)
    {
        var notification = new Notification
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            NotificationText = notificationText,
            SentAt = DateTime.UtcNow,
        };
        await _notificationDal.AddAsync(notification);
    }

    public async Task RemoveNotificationAsync(int notificationId)
    {
        var notifications = await _notificationDal.GetListAsync();
        var notification = notifications.FirstOrDefault(n => n.Id == notificationId);
        await _notificationDal.DeleteAsync(notification);
    }

    public async Task RemovingAllNotificationsOfUserAsync(string userId)
    {
        var notifications = await _notificationDal.GetListAsync();
        for (int i = 0; i < notifications.Count; i++)
        {
            if (notifications[i].ReceiverId == userId) await _notificationDal.DeleteAsync(notifications[i]);
        }
    }
}
