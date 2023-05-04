namespace Messenger.WebAPI.Shared.SharedModels;

public class EditMessageContext
{
    public long EditedMessageId { get; set; }
    public required DateTime EditDate { get; set; }
    public required string ChatId { get; set; }
}