using Messenger.Domain.Models;
using Messenger.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Domain.Services.Impl
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        public MessageService(IMessageRepository messageRepository, IUserRepository userRepository)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        public async Task DeleteMessageAsync(long messageId, bool isForAll, int userId)
        {
            if (isForAll)
            {
                var message = await _messageRepository.GetMessageByIdAsync(messageId);
                message.IsDeleted = true;
                await _messageRepository.EditMessageByIdAsync(messageId, message);

                return;
            }

            var user = _userRepository.GetUserByIdAsync(userId);
            DeletedMessage deletedMessage = new DeletedMessage()
            {
                MessageId = messageId,
                UserId = user.Id,
            };
            await _messageRepository.DeleteMessageForUserAsync(deletedMessage.Id);
        }

        public Task<bool> EditMessageAsync(long messageId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ForwardMessageAsync(long messageId, int chatId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ReadMessageAsync(long messageId, int userId)
        {
            var message = await _messageRepository.GetMessageByIdAsync(messageId);

            var readMessage = new ReadMessage()
            {
                MessageId = messageId,
                UserId = userId
            };
            _messageRepository.CreateReadMessageAsync(readMessage.Id);
        }

        public Task<bool> ReplyMessageAsync(long messageId)
        {
            throw new NotImplementedException();
        }

        public async Task SendMessageAsync(Chat chat, User user, byte[] content)
        {
            Message newMessage = new Message()
            {
                ChatId = chat.Id,
                DateOfDispatch = DateTime.Now,
                IsDeleted = false,
                IsEdited = false,
                SenderId = user.Id,
                Content = new MessageContent()
                {
                    /// убрать TypeId = 1 в будущем
                    Content = content,
                    TypeId = 1
                }
            };

            await _messageRepository.CreateMessageAsync(newMessage);
        }
    }
}
