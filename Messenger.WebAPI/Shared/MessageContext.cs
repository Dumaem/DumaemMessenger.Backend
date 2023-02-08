namespace Messenger.WebAPI.Shared;

public class MessageContext
{
    public required int ChatId { get; set; }
    public required object Message { get; set; }
}