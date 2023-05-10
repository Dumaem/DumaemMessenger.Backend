namespace Messenger.Domain.Models;

public class Chat
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsPersonal { get; set; }
}
