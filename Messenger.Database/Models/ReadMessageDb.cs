using System;
using System.Collections.Generic;

namespace Messenger.Database.Models;

public partial class ReadMessageDb
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public long MessageId { get; set; }

    public virtual MessageDb Message { get; set; } = null!;

    public virtual UserDb User { get; set; } = null!;
}
