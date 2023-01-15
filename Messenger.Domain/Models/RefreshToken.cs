namespace Messenger.Domain.Models;

public class RefreshToken
{
    public int Id { get; set; }

    /// <summary>
    /// Server-generated token Id used on client
    /// </summary>
    public string Token { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Access token Id that is related to this refresh token
    /// </summary>
    public string JwtId { get; set; } = null!;
    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int UserId { get; set; }
}