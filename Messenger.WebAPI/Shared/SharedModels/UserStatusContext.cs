using Messenger.Domain.Enums;

namespace Messenger.WebAPI.Shared.SharedModels;

public class UserStatusContext
{
    public int UserId { get; set; }
    public UserOnlineStatus Status { get; set; }
}