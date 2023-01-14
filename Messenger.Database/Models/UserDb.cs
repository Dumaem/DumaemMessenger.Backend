using System;
using System.Collections.Generic;

namespace Messenger.Database.Models;

public partial class UserDb
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;
}
