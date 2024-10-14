using SocialNetworkProjectMVC.Core.Abstraction;
using SocialNetworkProjectMVC.Entities.Enums;

namespace SocialNetworkProjectMVC.Entities.Models;
public class FriendRequest : IEntity
{
    // primary key:
    public int Id { get; set; }

    // foreign keys:
    public string ? SenderId { get; set; }  // the user who sends the friend request
    public string ? ReceiverId { get; set; }  // the user who is being friended

    // navigation properties:
    public virtual CustomIdentityUser? Sender { get; set; }
    public virtual CustomIdentityUser? Receiver { get; set; }

    // additional properties:
    public DateTime RequestDate { get; set; }  // when the friend request was made
    public DateTime AcceptedDate { get; set; }  // when the request was accepted (null if not yet accepted)
    public RequestStatus Status { get; set; } // status of the friendship (accepted or pending)
}
