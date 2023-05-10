namespace Messenger.Domain.Models;

public class Chat
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string? GroupName { get; set; }
    public bool Notifications { get; set; }
    public bool IsPersonal { get; set; }
}
