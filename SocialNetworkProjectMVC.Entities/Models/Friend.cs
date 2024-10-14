using SocialNetworkProjectMVC.Core.Abstraction;

namespace SocialNetworkProjectMVC.Entities.Models;
public class Friend : IEntity
{
    // foreign key : 
    public int Id { get; set; }

    // foreign keys : 
    public string? YourFriendId { get; set; }
    public string? OwnId { get; set; }

    // navigation properties :
    public virtual CustomIdentityUser? YourFriend { get; set; }
    public virtual CustomIdentityUser ? Own { get; set; }
}
