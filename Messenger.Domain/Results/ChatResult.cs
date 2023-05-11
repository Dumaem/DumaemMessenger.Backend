using Messenger.Domain.Models;

namespace Messenger.Domain.Results;

public class ChatResult : BaseResult
{
    public Chat Chat { get; set; } = null!;
}