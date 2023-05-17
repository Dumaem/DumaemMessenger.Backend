using System.Data;
using Dapper;
using FluentValidation;
using Messenger.Database.Models;
using Messenger.Database.Read;
using Messenger.Database.Read.Queries;
using Messenger.Database.Write;
using Messenger.Domain.Exception;
using Messenger.Domain.Models;
using Messenger.Domain.Repositories;
using Messenger.Domain.Results;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Database.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly MessengerContext _context;
    private readonly MessengerReadonlyContext _readonlyContext;

    public MessageRepository(MessengerContext context, MessengerReadonlyContext readonlyContext)
    {
        _context = context;
        _readonlyContext = readonlyContext;
    }

    public async Task<ListDataResult<Message>> ListMessagesAsync(string chatId, int userId, int count, int offset)
    {
        var chat = await _context.Chats.FirstOrDefaultAsync(x => x.Guid == chatId) ?? throw new NotFoundException();
        var messages = _context.Messages
            .Include(x => x.MessageContent)
            .Include(x => x.Sender)
            .Where(x => x.ChatId == chat.Id
                        && !x.IsDeleted
                        && x.DeletedMessages
                            .All(y => y.UserId != userId)
            );
        var result = messages
            .OrderByDescending(x => x.DateOfDispatch)
            .Skip(offset)
            .Take(count);
        return new ListDataResult<Message>
        {
            Success = true, Items = EntityConverter.ConvertMessages(result),
            TotalItemsCount = messages.Count()
        };
    }

    public async Task<ListDataResult<Message>> ListMessagesAsync(string chatId, int userId,
        int initialCount, int count, int offset)
    {
        var chat = await _context.Chats.FirstOrDefaultAsync(x => x.Guid == chatId) ?? throw new NotFoundException();
        var messages = _context.Messages
            .Include(x => x.MessageContent)
            .Include(x => x.Sender)
            .OrderByDescending(x => x.DateOfDispatch)
            .Take(initialCount)
            .Where(x => x.ChatId == chat.Id
                        && !x.IsDeleted
                        && x.DeletedMessages
                            .All(y => y.UserId != userId)
            );
        var result = messages
            .Skip(offset)
            .Take(count);
        return new ListDataResult<Message>
        {
            Success = true, Items = EntityConverter.ConvertMessages(result),
            TotalItemsCount = messages.Count()
        };
    }

    public async Task<string> GetShortMessagePreview(long messageId)
    {
        return (await _context.Messages
            .Include(x => x.MessageContent)
            .FirstAsync(x => x.Id == messageId)).MessageContent.Content[..20];
    }

    public async Task<string> GetChatNameFromMessage(long messageId)
    {
        return (await _context.Messages
                    .Include(x => x.Chat)
                    .FirstOrDefaultAsync(x => x.Id == messageId) ??
                throw new NotFoundException()).Chat.Guid;
    }

    public async Task<long> CreateMessageAsync(Message message, string chatId)
    {
        var chat = await _context.Chats
            .FirstOrDefaultAsync(x => x.Guid == chatId) ?? throw new NotFoundException();
        var dbMessage = new MessageDb
        {
            Id = message.Id,
            ChatId = chat.Id,
            DateOfDispatch = message.DateOfDispatch,
            SenderId = message.SenderId,
            ForwardedMessageId = message.ForwardedMessageId,
            RepliedMessageId = message.RepliedMessageId,
        };

        var dbMessageContent = new MessageContentDb
        {
            Message = dbMessage,
            MessageId = dbMessage.Id,
            Content = message.Content.Content,
            TypeId = message.Content.TypeId
        };

        _context.Messages.Add(dbMessage);
        _context.MessageContents.Add(dbMessageContent);

        await _context.SaveChangesAsync();
        return dbMessage.Id;
    }

    public async Task EditMessageByIdAsync(long id, Message editedMessage)
    {
        var message = _context.Messages.SingleOrDefault(m => m.Id == id) ??
                      throw new ValidationException("No message found");

        var messageContent = _context.MessageContents.SingleOrDefault(mc => mc.MessageId == message.Id) ??
                             throw new DataException($"No message content for message {id}. Check database issues");

        message.IsEdited = true;

        messageContent.Content = editedMessage.Content.Content;
        messageContent.TypeId = editedMessage.Content.TypeId;

        _context.Messages.Update(message);
        _context.MessageContents.Update(messageContent);

        await _context.SaveChangesAsync();
    }

    public async Task<Message> GetMessageByIdAsync(long id)
    {
        var res = (await _readonlyContext.Connection.QueryAsync<MessageContentDb, MessageDb, MessageContentDb>(
                      MessageRepositoryQueries.GetMessage,
                      (messageContent, message) =>
                      {
                          messageContent.Message = message;
                          return messageContent;
                      },
                      splitOn: "m_id", param: new {id})).SingleOrDefault() ??
                  throw new ValidationException($"No message with id {id} found");

        return new Message
        {
            Id = res.MessageId,
            ChatId = res.Message.ChatId,
            DateOfDispatch = res.Message.DateOfDispatch,
            IsDeleted = res.Message.IsDeleted,
            IsEdited = res.Message.IsEdited,
            SenderId = res.Message.SenderId,
            ForwardedMessageId = res.Message.ForwardedMessageId,
            RepliedMessageId = res.Message.RepliedMessageId,
            Content = new MessageContent
                {Content = res.Content, MessageId = res.MessageId, TypeId = res.TypeId}
        };
    }

    public async Task DeleteMessageForUserAsync(long deletedMessageId, int userId)
    {
        if (_context.Messages.FirstOrDefault(x => x.Id == deletedMessageId) is null)
            throw new NotFoundException();

        var deletedMessageDb = new DeletedMessageDb
        {
            MessageId = deletedMessageId,
            UserId = userId
        };

        _context.DeletedMessages.Add(deletedMessageDb);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteMessageForAllUsers(long deletedMessageId)
    {
        await _readonlyContext.Connection.QuerySingleOrDefaultAsync<MessageDb>(
            MessageRepositoryQueries.DeleteMessageForAllUsers, new {id = deletedMessageId});
    }

    public async Task CreateReadMessage(long messageId, int userId)
    {
        var readMessageDb = new ReadMessageDb
        {
            MessageId = messageId,
            UserId = userId
        };
        _context.ReadMessages.Add(readMessageDb);
        await _context.SaveChangesAsync();
    }
}