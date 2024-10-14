using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetworkProjectMVC.Entities.Models;

namespace SocialNetworkProjectMVC.Entities.Data;
public class ZustDBContext : IdentityDbContext<CustomIdentityUser, CustomIdentityRole, string>
{
    // parametric constructor for injecting options from 'program.cs' :
    public ZustDBContext(DbContextOptions<ZustDBContext> options) : base(options) { }

    // override OnModelCreating to configure relationships
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the FriendRequest relationship with Sender to cascade delete
        modelBuilder.Entity<FriendRequest>()
            .HasOne(fr => fr.Sender)
            .WithMany(u => u.FriendRequests)
            .HasForeignKey(fr => fr.SenderId)
            .OnDelete(DeleteBehavior.Cascade);  // Cascade delete to delete friend requests when Sender is deleted

        // Configure the FriendRequest relationship with Receiver to restrict delete
        modelBuilder.Entity<FriendRequest>()
            .HasOne(fr => fr.Receiver)
            .WithMany()  // Assuming you haven't defined a navigation property for Receiver
            .HasForeignKey(fr => fr.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);  // Restrict delete to avoid multiple cascade paths

        // Additional configurations (Friend, Chat, etc.)
        modelBuilder.Entity<Friend>()
            .HasKey(f => f.Id);

        modelBuilder.Entity<Friend>()
            .HasOne(f => f.Own)  // The user who owns this friendship
            .WithMany(u => u.Friends)  // CustomIdentityUser can have many Friends
            .HasForeignKey(f => f.OwnId)
            .OnDelete(DeleteBehavior.Cascade);  // Ensure delete behavior is as needed

        modelBuilder.Entity<Friend>()
            .HasOne(f => f.YourFriend)  // The friend in this friendship
            .WithMany()  // The friend might not have a collection of Friends, so no reverse navigation here
            .HasForeignKey(f => f.YourFriendId)
            .OnDelete(DeleteBehavior.Restrict);  // Ensure delete behavior is as needed

        // Configure the Chat relationship with User1 to cascade delete
        modelBuilder.Entity<Chat>()
            .HasOne(c => c.User1)
            .WithMany(u => u.Chats)  // Assuming each user has a collection of Chats
            .HasForeignKey(c => c.User1Id)
            .OnDelete(DeleteBehavior.Cascade);  // Cascade delete to delete chats when User1 is deleted

        // Configure the Chat relationship with User2 to restrict delete
        modelBuilder.Entity<Chat>()
            .HasOne(c => c.User2)
            .WithMany()
            .HasForeignKey(c => c.User2Id)
            .OnDelete(DeleteBehavior.Restrict);  // Restrict delete to avoid cascade conflicts

        // Configure Message relationships
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ChatId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Notification relationships
        modelBuilder.Entity<Notification>()
            .HasKey(n => n.Id); // Set primary key for Notification

        // Configure Receiver relationship
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.Receiver)  // Each notification has a receiver
            .WithMany(u => u.ReceivedNotifications)  // Assuming CustomIdentityUser has a collection of received notifications
            .HasForeignKey(n => n.ReceiverId)  // Foreign key for Receiver
            .OnDelete(DeleteBehavior.Cascade);  // Restrict delete behavior

        // Configure Sender relationship
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.Sender)  // Each notification has a sender
            .WithMany(u => u.SentNotifications)  // Assuming CustomIdentityUser has a collection of sent notifications
            .HasForeignKey(n => n.SenderId)  // Foreign key for Sender
            .OnDelete(DeleteBehavior.Restrict);  // Restrict delete behavior
    }


    // additional DbSets for your entities (Posts, Comments, Likes, etc.) :
    public virtual DbSet<Post> Posts { get; set; }
    public virtual DbSet<Comment> Comments { get; set; }
    public virtual DbSet<LikePost> LikesPosts { get; set; }
    public virtual DbSet<LikeComment> LikesComments { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<Chat> Chats { get; set; }
    public virtual DbSet<Friend> Friends { get; set; }
    public virtual DbSet<FriendRequest> FriendRequests { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }
}
