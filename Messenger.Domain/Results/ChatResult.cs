using Messenger.Domain.Models;

namespace Messenger.Domain.Results;

public class ChatResult : BaseResult
{
    public int ChatId { get; set; }
    public string ChatName { get; set; } = null!;
    public string? SenderName { get; set; }
    public Message? LastMessage { get; set; }
}