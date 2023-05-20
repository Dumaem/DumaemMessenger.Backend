namespace Messenger.Database.Models;

public class UserDb
{
    public int Id { get; set; }

    public string? Username { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool IsVerified { get; set; }

    public virtual ICollection<DeletedMessageDb> DeletedMessages { get; } = new List<DeletedMessageDb>();

    public virtual ICollection<MessageDb> Messages { get; } = new List<MessageDb>();

    public virtual ICollection<ReadMessageDb> ReadMessages { get; } = new List<ReadMessageDb>();

    public virtual ICollection<RefreshTokenDb> RefreshTokens { get; } = new List<RefreshTokenDb>();

    public virtual ICollection<UserChatDb> UserChats { get; } = new List<UserChatDb>();
}