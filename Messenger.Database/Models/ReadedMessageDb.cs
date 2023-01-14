namespace Messenger.Database.Models;

public partial class ReadMessageDb
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public long MessageId { get; set; }
}
