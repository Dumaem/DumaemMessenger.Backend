using System;
using System.Collections.Generic;

namespace Messenger.Database.Models;

public partial class UserChat
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ChatId { get; set; }
}
