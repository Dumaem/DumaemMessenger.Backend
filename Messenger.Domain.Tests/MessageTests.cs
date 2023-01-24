using Messenger.Domain.Models;
using Messenger.Domain.Repositories;
using Messenger.Domain.Services;
using Messenger.Domain.Services.Impl;
using Messenger.Domain.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Domain.Tests
{
    public class MessageTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMessageRepository> _messageRepositoryMock;

        private readonly IMessageService _messageService;

        public MessageTests() 
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _messageRepositoryMock= new Mock<IMessageRepository>();

            _messageService = new MessageService(_messageRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task SendMessageAsync_SuccessfullSending_ShouldReturnSuccessfulResult()
        {
            _messageRepositoryMock.Setup(x => x.CreateMessageAsync(new Message())).ReturnsAsync(It.IsAny<long>());

            Func<Task> res = async () => await _messageService.SendMessageAsync(It.IsAny<int>(), 
                It.IsAny<int>(), It.IsAny<byte[]>(), It.IsAny<int>());

            await res.Should().NotThrowAsync();
        }

        [Fact]
        public async Task DeleteMessageForOneUserAsync_SuccessfullDeleting_ShouldReturnSuccessfulResult()
        {
            _messageRepositoryMock.Setup(x => x.DeleteMessageForUserAsync(It.IsAny<long>(),
                It.IsAny<int>())).
                Returns(Task.CompletedTask);

            Func<Task> res = async () => await _messageService.DeleteMessageAsync(It.IsAny<long>(),
                It.IsAny<int>());

            await res.Should().NotThrowAsync();
        }

        [Fact]
        public async Task DeleteMessageForAllUsersAsync_SuccessfullDeleting_ShouldReturnSuccessfulResult()
        {
            _messageRepositoryMock.Setup(
                x => x.GetMessageByIdAsync(It.IsAny<long>())).ReturnsAsync(new Message());

            Func<Task> res = async () => await _messageService.DeleteMessageAsync(It.IsAny<long>(), (int?)null);

            // Only exception that this test falls in is NullReference one. This is becuase of userId nullability
            await res.Should().NotThrowAsync();
        }

        [Fact]
        public async Task ReadMessage_SuccessfullReading_ShouldReturnSuccessfulResult()
        {
            _messageRepositoryMock.Setup(x => x.CreateReadMessage(It.IsAny<long>(), It.IsAny<int>())).
                Returns(Task.CompletedTask);

            Func<Task> res = async () => await _messageService.ReadMessageAsync(It.IsAny<long>(),
                It.IsAny<int>());

            await res.Should().NotThrowAsync();
        }

        [Fact]
        public async Task ReplyMessage_SuccessfullReplying_ShouldReturnSuccessfulResult()
        {
            _messageRepositoryMock.Setup(
                x => x.CreateMessageAsync(new Message())).ReturnsAsync(It.IsAny<long>);

            Func<Task> res = async () => await _messageService.ReplyMessageAsync(It.IsAny<long>(),
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<byte[]>(), It.IsAny<int>());

            await res.Should().NotThrowAsync();
        }

        [Fact]
        public async Task ForwardMessage_SuccessfullForwarding_ShouldReturnSuccessfulResult()
        {
            _messageRepositoryMock.Setup(
                x => x.CreateMessageAsync(new Message())).ReturnsAsync(It.IsAny<long>);

            Func<Task> res = async () => await _messageService.ForwardMessageAsync(It.IsAny<long>(),
                It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());

            await res.Should().NotThrowAsync();
        }

        [Fact]
        public async Task EditMessage_SuccessfullEditing_ShouldReturnSuccessfulResult()
        {
            _messageRepositoryMock.Setup(
                x => x.GetMessageByIdAsync(It.IsAny<long>())).ReturnsAsync(new Message());

            Func<Task> res = async () => await _messageService.EditMessageAsync(It.IsAny<long>(),
                It.IsAny<byte[]>(), It.IsAny<int>());

            await res.Should().NotThrowAsync();
        }

        [Fact]
        public async Task EditMessage_UnsuccessfullEditing_ShouldReturnUnsuccessfulResult()
        {
            _messageRepositoryMock.Setup(
                x => x.GetMessageByIdAsync(It.IsAny<long>())).ReturnsAsync(() => null);

            Func<Task> res = async () => await _messageService.EditMessageAsync(It.IsAny<long>(),
                It.IsAny<byte[]>(), It.IsAny<int>());

            await res.Should().ThrowAsync<NullReferenceException>();
        }

    }
}
