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

    public static Message ConvertMessage(MessageDb messageDb)
    {
        return new Message
        {
            Id = messageDb.Id,
            DateOfDispatch = messageDb.DateOfDispatch,
            IsEdited = messageDb.IsEdited,
            IsDeleted = messageDb.IsDeleted,
            SenderId = messageDb.SenderId,
            ChatId = messageDb.ChatId,
            RepliedMessageId = messageDb.RepliedMessageId,
            ForwardedMessageId = messageDb.ForwardedMessageId,
            Content = ConvertMessageContent(messageDb.MessageContent)
        };
    }

    public static MessageContent ConvertMessageContent(MessageContentDb messageContentDb)
    {
        return new MessageContent
        {
            Id = messageContentDb.Id,
            Content = messageContentDb.Content,
            TypeId = messageContentDb.TypeId,
            MessageId = messageContentDb.MessageId
        };
    }
}