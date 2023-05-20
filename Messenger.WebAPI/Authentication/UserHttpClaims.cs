namespace Messenger.WebAPI.Authentication;

public class UserHttpClaims
{
    public int Id { get; init; }
    public string Email { get; init; } = null!;
}