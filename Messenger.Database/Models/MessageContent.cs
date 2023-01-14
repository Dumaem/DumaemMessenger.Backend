using System;
using System.Collections.Generic;

namespace Messenger.Database.Models;

public partial class MessageContent
{
    public long Id { get; set; }

    public byte[] Content { get; set; } = null!;

    public long MessageId { get; set; }

    public int TypeId { get; set; }
}
