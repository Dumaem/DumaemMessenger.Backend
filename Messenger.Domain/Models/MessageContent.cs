namespace Messenger.Domain.Models;

public class MessageContent
{
    public long Id { get; set; }

    public string Content { get; set; } = null!;

    public int TypeId { get; set; }

    public long MessageId { get; set; }
}
