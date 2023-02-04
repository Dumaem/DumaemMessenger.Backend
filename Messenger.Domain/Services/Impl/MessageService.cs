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

        public async Task SendMessageAsync(int chatId, int userId, byte[] content, int messageTypeId)
        {
            // Use the repository to persist the message to the data store

            var message = new Message
            {
                ChatId = chatId,
                SenderId = userId,
                Content = new MessageContent { Content = content, TypeId = messageTypeId },
                DateOfDispatch = DateTime.UtcNow
            };
            message.Content.MessageId = message.Id;
            await _messageRepository.CreateMessageAsync(message);
        }

        public async Task DeleteMessageAsync(long messageId, int? userId = null)
        {
            var message = await _messageRepository.GetMessageByIdAsync(messageId);

            // Use the repository to delete message for all users
            if (userId is null)
            {
                message.IsDeleted = true;
                await _messageRepository.DeleteMessageForAllUsers(messageId);
                return;
            }
            // Use the repository to delete the message for one user
            await _messageRepository.DeleteMessageForUserAsync(messageId, (int)userId);
        }

        public async Task ReadMessageAsync(long messageId, int userId)
        {
            // Use the repository to mark the message as read
            await _messageRepository.CreateReadMessage(messageId, userId);
        }

        public async Task ReplyMessageAsync(long repliedMessageId, int repliedMessageChatId,
            int newMessageSenderId, byte[] content, int messageTypeId)
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
            reply.Content.MessageId = reply.Id;
            await _messageRepository.CreateMessageAsync(reply);
        }

        public async Task ForwardMessageAsync(long forwardedMessageId, byte[] content, int messageTypeId, int chatId, int userId)
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
            forwardedMessage.Content.MessageId = forwardedMessage.Id;
            await _messageRepository.CreateMessageAsync(forwardedMessage);
        }

        public async Task EditMessageAsync(long messageId, byte[] content, int messageTypeId)
        {
            var message = await _messageRepository.GetMessageByIdAsync(messageId);
            // Use the repository to update the message's content
            message.Content = new MessageContent { Content = content, TypeId = messageTypeId };
            await _messageRepository.EditMessageByIdAsync(messageId, message);
        }
    }
}
