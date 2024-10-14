using SocialNetworkProjectMVC.Core.Abstraction;

namespace SocialNetworkProjectMVC.Entities.Models;
public class LikePost : IEntity
{
    // primary key :
    public int Id { get; set; }

    // foreign keys :
    public string ? UserId { get; set; }
    public int PostId { get; set; }

    // navigation properties :
    public virtual CustomIdentityUser ? User { get; set; }
    public virtual Post ? Post { get; set; }

    // additional properties : 
    public DateTime LikedAt { get; set; }
}
