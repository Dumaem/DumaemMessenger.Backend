﻿namespace Messenger.Database.Models;

public class RefreshTokenDb
{
    public int Id { get; set; }

    /// <summary>
    /// Server-generated token Id used on client
    /// </summary>
    public string Token { get; init; } = null!;

    /// <summary>
    /// Access token Id that is related to this refresh token
    /// </summary>
    public string JwtId { get; set; } = null!;

    public bool IsUsed { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public int UserId { get; set; }
    
    public string DeviceId { get; set; } = null!;

    public virtual UserDb User { get; set; } = null!;
}