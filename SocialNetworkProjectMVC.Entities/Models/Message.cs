using SocialNetworkProjectMVC.Core.Abstraction;

namespace SocialNetworkProjectMVC.Entities.Models;
public class Message : IEntity
{
    // primary key :
    public int Id { get; set; }

    // foreign keys:
    public string? SenderId { get; set; }  // the user who sends the message
    public string? ReceiverId { get; set; }  // the user who receives the message (friend or other user)
    public int ChatId { get; set; }

    // navigation properties (refer to CustomIdentityUser, not Friend) :
    public virtual CustomIdentityUser? Sender { get; set; }  // navigation property for sender
    public virtual CustomIdentityUser? Receiver { get; set; }  // navigation property for receiver (or friend)
    public virtual Chat ? Chat { get; set; }

    // additional properties:
    public string? MessageText { get; set; }  // the actual message content
    public DateTime SentAt { get; set; }  // timestamp when the message was sent
    public bool IsRead { get; set; }  // whether the message has been read
}