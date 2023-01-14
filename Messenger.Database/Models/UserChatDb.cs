namespace Messenger.Database.Models;

public partial class UserChatDb
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ChatId { get; set; }
}
