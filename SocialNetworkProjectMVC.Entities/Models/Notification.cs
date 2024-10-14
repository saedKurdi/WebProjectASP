using SocialNetworkProjectMVC.Core.Abstraction;

namespace SocialNetworkProjectMVC.Entities.Models;
public class Notification : IEntity
{
    // primary key : 
    public int Id { get; set; }

    // navigation properties :
    public virtual CustomIdentityUser ? Receiver { get; set; }
    public virtual CustomIdentityUser ? Sender { get; set; }

    // foreign keys :
    public string ? ReceiverId { get; set; }
    public string ? SenderId { get; set; }

    // additional properties : 
    public DateTime SentAt { get; set; }
    public string ? NotificationText { get; set; }
}
