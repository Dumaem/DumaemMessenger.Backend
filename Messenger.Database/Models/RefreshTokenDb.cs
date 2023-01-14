using System;
using System.Collections.Generic;

namespace Messenger.Database.Models;

public partial class RefreshTokenDb
{
    public int Id { get; set; }

    public string Token { get; set; } = null!;

    public string JwtId { get; set; } = null!;

    public bool IsUsed { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public int UserId { get; set; }

    public virtual UserDb User { get; set; } = null!;
}
