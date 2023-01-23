using Messenger.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Domain.Repositories
{
    public interface IMessageRepository
    {
        Task<long> CreateMessageAsync(Message message);
        Task<Message> GetMessageByIdAsync(long id);
        Task EditMessageByIdAsync(long id, Message editedMessage);
        Task DeleteMessageForUserAsync(long deletedMessageId, int userId);
        Task DeleteMessageForAllUsers(long deletedMessageId);
        Task CreateReadMessage(long messageId, int userId);
    }
}
