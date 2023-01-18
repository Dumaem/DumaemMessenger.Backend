namespace Messenger.Domain.Models;

public class MessageContent
{
    public long Id { get; set; }

    public byte[] Content { get; set; } = null!;

    public int TypeId { get; set; }
}
