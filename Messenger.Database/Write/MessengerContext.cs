using System;
using System.Collections.Generic;
using Messenger.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Database.Write;

public partial class MessengerContext : DbContext
{
    public MessengerContext()
    {
    }

    public MessengerContext(DbContextOptions<MessengerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<DeletedMessage> DeletedMessages { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<MessageContent> MessageContents { get; set; }

    public virtual DbSet<ReadedMessage> ReadedMessages { get; set; }

    public virtual DbSet<TypeContent> TypeContents { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserChat> UserChats { get; set; }

    public virtual DbSet<VersionInfo> VersionInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=26.204.218.207;Port=5432;Database=messenger;Username=readonly;Password=readonly;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.ToTable("chat");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<DeletedMessage>(entity =>
        {
            entity.ToTable("deleted_message");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<Message>(entity =>
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

        modelBuilder.Entity<MessageContent>(entity =>
        {
            entity.ToTable("message_content");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.TypeId).HasColumnName("type_id");
        });

        modelBuilder.Entity<ReadedMessage>(entity =>
        {
            entity.ToTable("readed_message");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<TypeContent>(entity =>
        {
            entity.ToTable("type_content");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Username).HasColumnName("username");
        });

        modelBuilder.Entity<UserChat>(entity =>
        {
            entity.ToTable("user_chat");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<VersionInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("VersionInfo");

            entity.HasIndex(e => e.Version, "UC_Version").IsUnique();

            entity.Property(e => e.AppliedOn).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Description).HasMaxLength(1024);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
