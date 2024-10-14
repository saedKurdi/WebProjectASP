using SocialNetworkProjectMVC.Core.Abstraction;

namespace SocialNetworkProjectMVC.Entities.Models;
public class Comment : IEntity
{
    // primary key : 
    public int Id { get; set; }

    // additional properties : 
    public string ? CommentText { get; set; }
    public DateTime CreatedAt { get; set; }

    // navigation properties : 
    public virtual CustomIdentityUser ? User { get; set; }
    public virtual Post ? Post { get; set; }
    public virtual ICollection<LikeComment> ? Likes { get; set; }
    
    // foreign keys : 
    public string ? UserId { get; set; }
    public int PostId { get; set; }

    // default constructor for initializing the navigation properties :
    public Comment()
    {
        Likes = new List<LikeComment>();
    }
}
