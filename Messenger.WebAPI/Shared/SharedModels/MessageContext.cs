namespace Messenger.WebAPI.Shared.SharedModels;

public class MessageContext
{
    public required object Content { get; set; }
    public required int UserId { get; set; }
    public required DateTime SendDate { get; set; }
    public required string ChatId { get; set; }
    public int? ForwardedMessageId { get; set; }
    public int? RepliedMessageId { get; set; }
}