using System;
using System.Collections.Generic;

namespace Messenger.Database.Models;

public partial class ContentTypeDb
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<MessageContentDb> MessageContents { get; } = new List<MessageContentDb>();
}
