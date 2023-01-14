namespace Messenger.Domain.Models;

public class ReadMessageDb
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public long MessageId { get; set; }
}
