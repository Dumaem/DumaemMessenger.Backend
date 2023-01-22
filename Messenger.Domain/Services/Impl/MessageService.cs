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

        public async Task<bool> SendMessageAsync(int chatId, int userId, byte[] content, int messageTypeId)
        {
            // Use the repository to persist the message to the data store

            var message = new Message
            {
                ChatId = chatId,
                SenderId = userId,
                Content = new MessageContent { Content = content, TypeId = messageTypeId },
                DateOfDispatch = DateTime.UtcNow
            };
            await _messageRepository.CreateMessageAsync(message);
            return true;
        }

        public async Task<bool> DeleteMessageAsync(long messageId, int? userId = null)
        {
            var message = await _messageRepository.GetMessageByIdAsync(messageId);

            // Use the repository to delete message for all users
            if (userId is null)
            {
                message.IsDeleted = true;
                await _messageRepository.EditMessageByIdAsync(messageId, message);
                return true;
            }
            // Use the repository to delete the message for one user
            await _messageRepository.DeleteMessageForUserAsync(messageId, userId);
            return true;
        }

        public async Task<bool> ReadMessageAsync(long messageId, int userId)
        {
            // Use the repository to mark the message as read
            ReadMessage readMessage = new ReadMessage()
            {
                MessageId = messageId,
                UserId = userId
            };
            await _messageRepository.CreateReadMessage(readMessage);
            return true;
        }

        public async Task<bool> ReplyMessageAsync(long repliedMessageId, int repliedMessageChatId,
            long newMessageId, int newMessageSenderId, byte[] content, int messageTypeId)
        {
            // Use the repository to send a reply message
            var reply = new Message
            {
                ChatId = repliedMessageChatId,
                SenderId = newMessageSenderId,
                Content = new MessageContent { Content = content, TypeId = messageTypeId },
                RepliedMessageId = repliedMessageId,
                DateOfDispatch = DateTime.UtcNow
            };
            await _messageRepository.CreateMessageAsync(reply);
            return true;
        }

        public async Task<bool> ForwardMessageAsync(long forwardedMessageId, byte[] content, int messageTypeId, int chatId, int userId)
        {
            // Use the repository to forward the message to another chat
            var forwardedMessage = new Message
            {
                ChatId = chatId,
                SenderId = userId,
                ForwardedMessageId = forwardedMessageId,
                Content = new MessageContent { Content = content, TypeId = messageTypeId },
                DateOfDispatch = DateTime.UtcNow
            };
            await _messageRepository.CreateMessageAsync(forwardedMessage);
            return true;
        }

        public async Task<bool> EditMessageAsync(long messageId, byte[] content, int messageTypeId)
        {
            var message = await _messageRepository.GetMessageByIdAsync(messageId);
            // Use the repository to update the message's content
            message.Content = new MessageContent { Content = content, TypeId = messageTypeId };
            await _messageRepository.EditMessageByIdAsync(messageId, message);
            return true;
        }
    }
}
