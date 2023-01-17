using Messenger.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Domain.Services.Impl
{
    public class MessageService : IMessageService
    {
        public MessageService()
        {

        }

        public Task<bool> DeleteMessageAsync(Message message, bool isForAll)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditMessageAsync(Message message)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ForwardMessageAsync(Message message, Chat chat)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReadMessageAsync(Message message)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReplyMessageAsync(Message message)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendMessageAsync(Chat chat)
        {
            throw new NotImplementedException();
        }
    }
}
