namespace SocialNetworkProjectMVC.Business.Abstract;
public interface INotificationService
{
    // methods will be implemented in class : 
    public Task AddNotificationAsync(string senderId,string receiverId,string notificationText);
    public Task RemoveNotificationAsync(int notificationId);
    public Task RemovingAllNotificationsOfUserAsync(string userId);
}
