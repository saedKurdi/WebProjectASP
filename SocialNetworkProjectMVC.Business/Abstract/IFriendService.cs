using SocialNetworkProjectMVC.Entities.Models;

namespace SocialNetworkProjectMVC.Business.Abstract;
public interface IFriendService
{
    // methods will be implemented in class : 
    public Task<List<Friend>> GetFriendsOfCurrentUserAsync(string key = "");
    public Task RemoveFriendAsync(string friendId);
    public Task AddFriendAsync(string friendId);
    public Task<List<CustomIdentityUser>> GetOtherPeopleAsync(string key = "");
}
