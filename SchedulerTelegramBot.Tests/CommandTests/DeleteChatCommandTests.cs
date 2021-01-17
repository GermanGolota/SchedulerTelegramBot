using Infrastructure.Repositories;
using Moq;
using System.Threading.Tasks;
using WebAPI.Commands;
using Xunit;
using Infrastructure.Exceptions;
using Telegram.Bot.Types;
using SchedulerTelegramBot.Tests.Mocks;
using System;
using WebAPI.Commands.Verifiers;

namespace SchedulerTelegramBot.Tests
{
    public class DeleteChatCommandTests : CommandTestBase
    {
        private DeleteChatCommand _sut;
        private readonly Mock<IChatRepo> _repoMock = new Mock<IChatRepo>();
        private string SuccessMessage = StandardMessages.ChatDeletionSuccess;
        public DeleteChatCommandTests()
        {
            _sut = new DeleteChatCommand(_clientMock.Object,
                _repoMock.Object, new LoggerMock<DeleteChatCommand>());
        }
        private void AssertMessageBeenSend()
        {
            _clientMock.Verify(x => x.SendTextMessageAsync(It.IsAny<ChatId>(), It.IsAny<string>()), Times.Once);
        }

        private void SetupRepoToContainChat()
        {
            _repoMock.Setup(x => x.DeleteChat(It.IsAny<string>()));
        }

        [Fact]
        public async Task ExecuteCommandAsync_ShouldWork_ChatInSystem()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupRepoToContainChat();

            Update update = GetUpdate();

            //Act
            await _sut.Execute(update);
            //Assert
            AssertMessageBeenSend();
        }
        [Fact]
        public async Task ExecuteCommandAsync_ShouldFail_ChatNotInSystem()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupRepoToNotContainChat();

            Update update = GetUpdate();

            //Act
            await _sut.Execute(update);
            //Assert
            AssertMessageNotBeenSend();
        }


        private void AssertMessageNotBeenSend()
        {
            _clientMock.Verify(x => x.SendTextMessageAsync(It.IsAny<ChatId>(), SuccessMessage), Times.Never);
        }
        private void SetupRepoToNotContainChat()
        {
            var expectedException = new ChatDontExistException();
            _repoMock.Setup(x => x.DeleteChat(It.IsAny<string>())).Throws(expectedException);
        }
        private Update GetUpdate()
        {
            return new Update
            {
                Message = new Message
                {
                    Text = "/deleteChat",
                    Chat = new Chat
                    {
                        Id = long.Parse(TestChatId)
                    },
                    From = new User
                    {
                        Id = int.Parse(UserId)
                    }
                }
            };
        }
    }
}
