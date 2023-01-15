using System;
using System.Collections.Generic;

namespace Messenger.Database.Models;

public partial class UserChatDb
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ChatId { get; set; }

    public virtual UserDb User { get; set; } = null!;
}
