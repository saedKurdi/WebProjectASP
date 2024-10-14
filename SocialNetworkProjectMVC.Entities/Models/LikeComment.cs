using SocialNetworkProjectMVC.Core.Abstraction;

namespace SocialNetworkProjectMVC.Entities.Models;
public class LikeComment : IEntity
{
    // primary key :
    public int Id { get; set; }

    // foreign keys :
    public string ? UserId { get; set; }
    public int CommentId { get; set; }

    // navigation properties :
    public virtual CustomIdentityUser? User { get; set; }
    public virtual Comment ? Comment { get; set; }

    // additional properties : 
    public DateTime LikedAt { get; set; }
}
