namespace Messenger.WebAPI.Shared.SharedModels;

public class MessageReadContext
{
    public required int UserId { get; set; }
    public required long MessageId { get; set; }
    public required string ChatName { get; set; }
}