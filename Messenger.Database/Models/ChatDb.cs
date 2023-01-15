namespace Messenger.Database.Models;

public class ChatDb
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<MessageDb> Messages { get; } = new List<MessageDb>();
}
