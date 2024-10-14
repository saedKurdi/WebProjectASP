using SocialNetworkProjectMVC.Business.Abstract;
using SocialNetworkProjectMVC.DataAccess.Abstract;
using SocialNetworkProjectMVC.Entities.Models;

namespace SocialNetworkProjectMVC.Business.Concrete;
public class MessageService : IMessageService
{
    // private readonly fields for injection : 
    private readonly IMessageDal _messageDal;

    // parametric constructor for injecting :
    public MessageService(IMessageDal messageDal)
    {
        _messageDal = messageDal;
    }

    // implemented methods : 
    public async Task AddMessageAsync(int chatId, string senderId, string receiverId, string messageText)
    {
        var msg = new Message
        {
            ChatId = chatId,
            SenderId = senderId,
            ReceiverId = receiverId,
            MessageText = messageText,
            IsRead = false,
            SentAt = DateTime.Now,
        };
        await _messageDal.AddAsync(msg);
    }
}
