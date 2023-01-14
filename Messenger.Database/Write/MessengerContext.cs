using Messenger.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Database.Write;

public class MessengerContext : DbContext
{
    public MessengerContext()
    {
    }

    public MessengerContext(DbContextOptions<MessengerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChatDb> Chats { get; set; } = null!;

    public virtual DbSet<DeletedMessageDb> DeletedMessages { get; set; } = null!;

    public virtual DbSet<MessageDb> Messages { get; set; } = null!;

    public virtual DbSet<MessageContentDb> MessageContents { get; set; } = null!;

    public virtual DbSet<ReadMessageDb> ReadMessages { get; set; } = null!;

    public virtual DbSet<ContentTypeDb> ContentTypes { get; set; } = null!;

    public virtual DbSet<UserDb> Users { get; set; } = null!;

    public virtual DbSet<UserChatDb> UserChats { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatDb>(entity =>
        {
            entity.ToTable("chat");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<DeletedMessageDb>(entity =>
        {
            entity.ToTable("deleted_message");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<MessageDb>(entity =>
        {
            entity.ToTable("message");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.DateOfDispatch)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_of_dispatch");
            entity.Property(e => e.ForwardedMessageId).HasColumnName("forwarded_message_id");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.IsEdited).HasColumnName("is_edited");
            entity.Property(e => e.RepliedMessageId).HasColumnName("replied_message_id");
            entity.Property(e => e.SenderId).HasColumnName("sender_id");
        });

        modelBuilder.Entity<MessageContentDb>(entity =>
        {
            entity.ToTable("message_content");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.TypeId).HasColumnName("type_id");
        });

        modelBuilder.Entity<ReadMessageDb>(entity =>
        {
            entity.ToTable("read_message");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<ContentTypeDb>(entity =>
        {
            entity.ToTable("content_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<UserDb>(entity =>
        {
            entity.ToTable("user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Username).HasColumnName("username");
        });

        modelBuilder.Entity<UserChatDb>(entity =>
        {
            entity.ToTable("user_chat");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });
        
        modelBuilder.Entity<RefreshTokenDb>(entity =>
        {
            entity.ToTable("refresh_token");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Token).HasColumnName("token");
            entity.Property(e => e.JwtId).HasColumnName("jwt_id");
            entity.Property(e => e.IsUsed).HasColumnName("is_used");
            entity.Property(e => e.IsRevoked).HasColumnName("is_revoked");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });
    }
}
