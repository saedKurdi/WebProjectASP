using SocialNetworkProjectMVC.Business.Abstract;
using SocialNetworkProjectMVC.DataAccess.Abstract;
using SocialNetworkProjectMVC.Entities.Models;

namespace SocialNetworkProjectMVC.Business.Concrete;
public class ChatService : IChatService
{
    // private readonly fields for injection : 
    private readonly IChatDal _chatDal;

    // parametric constructor for injecting :
    public ChatService(IChatDal chatDal)
    {
        _chatDal = chatDal;
    }

    // implemented methods :  
    public async Task AddChatAsync(string user1Id, string user2Id)
    {
        var allChats = await _chatDal.GetListAsync();
        var chatExsists = allChats.Exists(c => c.User1Id == user1Id &&  c.User2Id == user2Id || c.User1Id == user2Id && c.User2Id == user1Id);
        if (!chatExsists)
        {
            var chat = new Chat
            {
                User1Id = user1Id,
                User2Id = user2Id,
            };
            await _chatDal.AddAsync(chat);
        } 
    }

    public async Task DeleteChatAsync(string user1Id, string user2Id)
    {
        var chat = await GetChatAsync(user1Id, user2Id);
        await _chatDal.DeleteAsync(chat);
    }

    public async Task<Chat> GetChatAsync(string user1Id,string user2Id)
    {
        await AddChatAsync(user1Id,user2Id);
        var chats = await _chatDal.GetListAsync();
        var chat = chats.FirstOrDefault(c => c.User1Id == user1Id && c.User2Id == user2Id || c.User1Id == user2Id && c.User2Id == user1Id);
        return chat;
    }

    public async Task<List<Chat>> GetChatsByReceiverOrSenderIdAsync(string id)
    {
        var chats = await _chatDal.GetListAsync();
        var conditionalChats = chats.Where(c => c.User1Id == id || c.User2Id == id).ToList();
        return conditionalChats;
    }
}
