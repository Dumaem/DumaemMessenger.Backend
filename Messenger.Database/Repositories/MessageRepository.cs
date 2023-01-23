using Dapper;
using FluentValidation;
using Messenger.Database.Models;
using Messenger.Database.Read;
using Messenger.Database.Read.Queries;
using Messenger.Database.Write;
using Messenger.Domain.Models;
using Messenger.Domain.Repositories;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Database.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MessengerContext _context;
        private readonly MessengerReadonlyContext _readonlyContext;

        public MessageRepository(MessengerContext context, MessengerReadonlyContext readonlyContext)
        {
            _context = context;
            _readonlyContext = readonlyContext;
        }

        public async Task<long> CreateMessageAsync(Message message)
        {
            var dbMessage = new MessageDb
            {
                Id = message.Id,
                ChatId = message.ChatId,
                DateOfDispatch = message.DateOfDispatch,
                IsDeleted = message.IsDeleted,
                IsEdited = message.IsEdited,
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
            var message = _context.Messages.SingleOrDefault(m => m.Id == id);
            var messageContent = _context.MessageContents.SingleOrDefault(mc => mc.MessageId == message.Id);

            message.IsEdited = true;

            messageContent.Content = editedMessage.Content.Content;
            messageContent.TypeId = editedMessage.Content.TypeId;

            _context.Messages.Update(message);
            _context.MessageContents.Update(messageContent);

            await _context.SaveChangesAsync();
        }

        public async Task<Message?> GetMessageByIdAsync(long id)
        {
            var res = (await _readonlyContext.Connection.QueryAsync<MessageContentDb, MessageDb, MessageContentDb>(MessageRepositoryQueries.GetMessage, 
                (messageContent, message) => {
                messageContent.Message = message;
                return messageContent;
            },
            splitOn: "c.Id", param: new {id})).SingleOrDefault();

            return res is null
                ? null
                : new Message
                {
                    ChatId = res.Message.ChatId,
                    DateOfDispatch = res.Message.DateOfDispatch,
                    IsDeleted = res.Message.IsDeleted,
                    IsEdited = res.Message.IsEdited,
                    SenderId = res.Message.SenderId,
                    ForwardedMessageId = res.Message.ForwardedMessageId,
                    RepliedMessageId = res.Message.RepliedMessageId,
                    Content = new MessageContent { Content = res.Content, MessageId= res.MessageId, TypeId = res.TypeId},
                };
        }

        public async Task DeleteMessageForUserAsync(long deletedMessageId, int userId)
        {
            var deletedMessage = _context.Messages.SingleOrDefault(m => m.Id == deletedMessageId) ?? throw new ValidationException("No message found");

            DeletedMessageDb deletedMessageDb = new DeletedMessageDb()
            {
                MessageId = deletedMessageId,
                UserId = userId
            };

            _context.DeletedMessages.Add(deletedMessageDb);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMessageForAllUsers(long deletedMessageId)
        {
            var res = await _readonlyContext.Connection.QuerySingleOrDefaultAsync<MessageDb>(
            MessageRepositoryQueries.DeleteMessageForAllUsers, new { id = deletedMessageId });
        }

        public async Task CreateReadMessage(long messageId, int userId)
        {
            ReadMessageDb readMessageDb = new ReadMessageDb()
            {
                MessageId = messageId,
                UserId = userId
            };
            _context.ReadMessages.Add(readMessageDb);
            await _context.SaveChangesAsync();
        }
    }
}
