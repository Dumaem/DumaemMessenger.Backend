namespace Messenger.Database.Models;

public class MessageContentDb
{
    public long Id { get; set; }

    public byte[] Content { get; set; } = null!;

    public long MessageId { get; set; }

    public int TypeId { get; set; }

    public virtual MessageDb Message { get; set; } = null!;

    public virtual ContentTypeDb Type { get; set; } = null!;
}
