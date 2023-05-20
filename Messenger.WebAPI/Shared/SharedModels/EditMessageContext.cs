namespace Messenger.WebAPI.Shared.SharedModels;

public class EditMessageContext
{
    public required int ContentType { get; set; }
    public required string Content { get; set; }
    public long MessageId { get; set; }
    public required string ChatId { get; set; }
}