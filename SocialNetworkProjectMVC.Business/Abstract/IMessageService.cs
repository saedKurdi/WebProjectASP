namespace SocialNetworkProjectMVC.Business.Abstract;
public interface IMessageService
{
    // methods will be implemented in class : 
    public Task AddMessageAsync(int chatId,string senderId, string receiverId, string messageText);
}
