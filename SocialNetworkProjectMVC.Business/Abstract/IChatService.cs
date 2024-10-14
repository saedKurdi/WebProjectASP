using SocialNetworkProjectMVC.Entities.Models;

namespace SocialNetworkProjectMVC.Business.Abstract;
public interface IChatService
{
    // methods will be implemented in class : 
    public Task AddChatAsync(string user1Id,string user2Id);
    public Task<Chat> GetChatAsync(string user1Id, string user2Id);
    public Task DeleteChatAsync(string user1Id,string user2Id);
    public Task<List<Chat>> GetChatsByReceiverOrSenderIdAsync(string id);
}
