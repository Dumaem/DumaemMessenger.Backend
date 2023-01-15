namespace Messenger.Database.Models;

public class ContentTypeDb
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<MessageContentDb> MessageContents { get; } = new List<MessageContentDb>();
}
