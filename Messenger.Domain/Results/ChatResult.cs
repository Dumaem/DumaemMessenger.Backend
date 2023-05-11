using Messenger.Domain.Models;

namespace Messenger.Domain.Results;

public class ChatResult : BaseResult
{
    public int ChatId { get; set; }
    public string ChatName { get; set; } = null!;
    public Message? LastMessage { get; set; }
}