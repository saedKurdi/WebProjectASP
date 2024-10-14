using SocialNetworkProjectMVC.Core.Abstraction;

namespace SocialNetworkProjectMVC.Entities.Models;
public class Chat : IEntity
{
    // foreign key : 
    public int Id { get; set; }

    // navigation properties : 
    public virtual CustomIdentityUser ? User1{ get; set; }
    public virtual CustomIdentityUser ? User2{ get; set; }
    public virtual ICollection<Message> ? Messages { get; set; }

    // foreign keys :
    public string ? User1Id { get; set; }
    public string ? User2Id { get; set; }

    // default constructor for initializing the navigation properties :
    public Chat()
    {
        Messages = new List<Message>();
    }
}
