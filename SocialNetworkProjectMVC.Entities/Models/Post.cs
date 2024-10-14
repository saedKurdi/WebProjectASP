using SocialNetworkProjectMVC.Core.Abstraction;
using SocialNetworkProjectMVC.Entities.Enums;

namespace SocialNetworkProjectMVC.Entities.Models;
public class Post : IEntity
{
    // primary key :
    public int Id { get; set; }

    // additional properties :
    public DateTime CreatedAt { get; set; }
    public bool IsHidden { get; set; }
    public string ? Description {  get; set; }
    public string ? Url { get; set; }
    public PostType PostType { get; set; }

    // navigation properties : 
    public virtual CustomIdentityUser ? User { get; set; }
    public virtual ICollection<LikePost> ? Likes { get; set; }
    public virtual ICollection<Comment> ? Comments { get; set; }
    
    // foreign keys : 
    public string ? UserId { get; set; }

    // default constructor for initializing the navigation properties :
    public Post()
    {
        Likes = new List<LikePost>();
        Comments = new List<Comment>();
    }
}
