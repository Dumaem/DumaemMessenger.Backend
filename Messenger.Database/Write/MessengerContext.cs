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

    public virtual DbSet<ContentTypeDb> ContentTypes { get; set; } = null!;

    public virtual DbSet<DeletedMessageDb> DeletedMessages { get; set; } = null!;

    public virtual DbSet<MessageDb> Messages { get; set; } = null!;

    public virtual DbSet<MessageContentDb> MessageContents { get; set; } = null!;

    public virtual DbSet<ReadMessageDb> ReadMessages { get; set; } = null!;

    public virtual DbSet<RefreshTokenDb> RefreshTokens { get; set; } = null!;

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

        modelBuilder.Entity<ContentTypeDb>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_type_content");

            entity.ToTable("content_type");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('type_content_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<DeletedMessageDb>(entity =>
        {
            entity.ToTable("deleted_message");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Message).WithMany(p => p.DeletedMessages)
                .HasForeignKey(d => d.MessageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_deleted_message_message_id_message_id");

            entity.HasOne(d => d.User).WithMany(p => p.DeletedMessages)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_deleted_message_user_id_user_id");
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

            entity.HasOne(d => d.Chat).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_message_chat_id_chat_id");

            entity.HasOne(d => d.ForwardedMessage).WithMany(p => p.InverseForwardedMessage)
                .HasForeignKey(d => d.ForwardedMessageId)
                .HasConstraintName("FK_message_forwarded_message_id_message_id");

            entity.HasOne(d => d.RepliedMessage).WithMany(p => p.InverseRepliedMessage)
                .HasForeignKey(d => d.RepliedMessageId)
                .HasConstraintName("FK_message_replied_message_id_message_id");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_message_sender_id_user_id");
        });

        modelBuilder.Entity<MessageContentDb>(entity =>
        {
            entity.ToTable("message_content");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.TypeId).HasColumnName("type_id");

            entity.HasOne(d => d.Message).WithMany(p => p.MessageContents)
                .HasForeignKey(d => d.MessageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_message_content_message_id_message_id");

            entity.HasOne(d => d.Type).WithMany(p => p.MessageContents)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_message_content_type_id_content_type_id");
        });

        modelBuilder.Entity<ReadMessageDb>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_readed_message");

            entity.ToTable("read_message");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('readed_message_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Message).WithMany(p => p.ReadMessages)
                .HasForeignKey(d => d.MessageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_read_message_message_id_message_id");

            entity.HasOne(d => d.User).WithMany(p => p.ReadMessages)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_read_message_user_id_user_id");
        });

        modelBuilder.Entity<RefreshTokenDb>(entity =>
        {
            entity.ToTable("refresh_token");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreationDate)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_date");
            entity.Property(e => e.ExpiryDate)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("expiry_date");
            entity.Property(e => e.IsRevoked).HasColumnName("is_revoked");
            entity.Property(e => e.IsUsed).HasColumnName("is_used");
            entity.Property(e => e.JwtId)
                .HasMaxLength(250)
                .HasColumnName("jwt_id");
            entity.Property(e => e.Token)
                .HasMaxLength(250)
                .HasColumnName("token");
            entity.Property(e => e.DeviceId)
                .HasMaxLength(250)
                .HasColumnName("device_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_refresh_token_user_id_user_id");
        });

        modelBuilder.Entity<UserDb>(entity =>
        {
            entity.ToTable("user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(32)
                .IsFixedLength()
                .HasColumnName("password");
            entity.Property(e => e.Username).HasColumnName("username");
        });

        modelBuilder.Entity<UserChatDb>(entity =>
        {
            entity.ToTable("user_chat");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserChats)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_chat_user_id_user_id");

            entity.HasOne(d => d.Chat).WithMany(p => p.Users)
                .HasForeignKey(d => d.ChatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_chat_chat_id_chat_id");
        });
    }
}
