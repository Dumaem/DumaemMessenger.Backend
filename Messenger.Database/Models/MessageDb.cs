namespace Messenger.Database.Models;

public class MessageDb
{
    public long Id { get; set; }

    public DateTime DateOfDispatch { get; set; }

    public bool IsEdited { get; set; }

    public bool IsDeleted { get; set; }

    public int SenderId { get; set; }

    public int ChatId { get; set; }

    public long? RepliedMessageId { get; set; }

    public long? ForwardedMessageId { get; set; }

    public virtual ChatDb Chat { get; set; } = null!;

    public virtual ICollection<DeletedMessageDb> DeletedMessages { get; } = new List<DeletedMessageDb>();

    public virtual MessageDb? ForwardedMessage { get; set; }

    public virtual ICollection<MessageDb> InverseForwardedMessage { get; } = new List<MessageDb>();

    public virtual ICollection<MessageDb> InverseRepliedMessage { get; } = new List<MessageDb>();

    public virtual ICollection<MessageContentDb> MessageContents { get; } = new List<MessageContentDb>();

    public virtual ICollection<ReadMessageDb> ReadMessages { get; } = new List<ReadMessageDb>();

    public virtual MessageDb? RepliedMessage { get; set; }

    public virtual UserDb Sender { get; set; } = null!;
}
