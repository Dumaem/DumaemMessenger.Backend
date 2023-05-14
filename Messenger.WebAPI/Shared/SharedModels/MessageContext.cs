using Messenger.Domain.Models;

namespace Messenger.WebAPI.Shared.SharedModels;

public class MessageContext
{
    public required int ContentType { get; set; }
    public required string Content { get; set; }
    public required int UserId { get; set; }
    public User? User { get; set; }
    public required DateTime SendDate { get; set; }
    public required string ChatId { get; set; }
    public long? ForwardedMessageId { get; set; }
    public long? RepliedMessageId { get; set; }
}