using Dapper;
using Messenger.Database.Models;
using Messenger.Database.Read;
using Messenger.Database.Read.Queries;
using Messenger.Database.Write;
using Messenger.Domain.Models;
using Messenger.Domain.Repositories;
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
                RepliedMessageId = message.RepliedMessageId
            };

            _context.Messages.Add(dbMessage);
            await _context.SaveChangesAsync();
            return dbMessage.Id;
        }

        public async Task<bool> EditMessageByIdAsync(long id, Message editedMessage)
        {
            var message  = _context.Messages.SingleOrDefault(m => m.Id == id);

            message.Id = editedMessage.Id;
            message.SenderId = editedMessage.SenderId;
            message.RepliedMessageId = editedMessage.RepliedMessageId;
            message.ForwardedMessageId= editedMessage.ForwardedMessageId;
            message.ChatId = editedMessage.ChatId;
            message.DateOfDispatch = editedMessage.DateOfDispatch;
            message.IsEdited = true;
            message.IsDeleted = editedMessage.IsDeleted;

            return true;
        }

        public async Task<Message?> GetMessageByIdAsync(long id)
        {
            var res = await _readonlyContext.Connection.QuerySingleOrDefaultAsync<MessageDb>(
                        MessageRepositoryQueries.GetMessage, new { id });

            return res is null
                ? null
                : new Message
                {
                    Id = res.Id,
                    ChatId= res.ChatId,
                    DateOfDispatch = res.DateOfDispatch,
                    IsDeleted= res.IsDeleted,
                    IsEdited= res.IsEdited,
                    SenderId= res.SenderId,
                    ForwardedMessageId= res.ForwardedMessageId,
                    RepliedMessageId = res.RepliedMessageId
                };
        }
    }
}
