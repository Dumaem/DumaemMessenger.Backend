namespace Messenger.WebAPI.Shared;

public class MessageContext
{
    public required string ChatId { get; set; }
    public required object Message { get; set; }
    public required int UserId { get; set; }
}