namespace Messenger.Domain.Models;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;
}
