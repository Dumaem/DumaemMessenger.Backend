using System;
using System.Collections.Generic;

namespace Messenger.Database.Models;

public partial class ChatDb
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<MessageDb> Messages { get; } = new List<MessageDb>();
}
