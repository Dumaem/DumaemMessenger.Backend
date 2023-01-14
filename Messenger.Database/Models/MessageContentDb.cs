﻿namespace Messenger.Database.Models;

public partial class MessageContentDb
{
    public long Id { get; set; }

    public byte[] Content { get; set; } = null!;

    public long MessageId { get; set; }

    public int TypeId { get; set; }
}
