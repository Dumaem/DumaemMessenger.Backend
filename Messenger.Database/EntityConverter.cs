using Messenger.Database.Models;
using Messenger.Domain.Models;

namespace Messenger.Database;

public static class EntityConverter
{
    public static IEnumerable<Message> ConvertMessages(IEnumerable<MessageDb> messagesDb)
    {
        return messagesDb.Select(res =>
        {
            var message = new Message
            {
                Id = res.Id,
                ChatId = res.ChatId,
                DateOfDispatch = res.DateOfDispatch,
                IsDeleted = res.IsDeleted,
                IsEdited = res.IsEdited,
                SenderId = res.SenderId,
                ForwardedMessageId = res.ForwardedMessageId,
                RepliedMessageId = res.RepliedMessageId,
                Content = new MessageContent
                    { Content = res.MessageContent.Content, TypeId = res.MessageContent.TypeId }
            };

            return message;
        });
    }
}