namespace Messenger.Database.Models;

public class UserChatDb
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ChatId { get; set; }

    public virtual UserDb User { get; set; } = null!;
    public virtual ChatDb Chat { get; set; } = null!;
}
