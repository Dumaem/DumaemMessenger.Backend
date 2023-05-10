﻿using Messenger.Domain.Models;

namespace Messenger.WebAPI.Credentials;

public class ChatCreateCredentials
{
    public IEnumerable<User> Participants { get; set; } = null!;
    public bool IsPersonal { get; set; }
    public User? CurrentUser { get; set; }
    public string? GroupName { get; set; }
}