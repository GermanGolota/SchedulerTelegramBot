using Infrastructure.Repositories;
using Moq;
using System.Threading.Tasks;
using WebAPI.Commands;
using Xunit;
using Infrastructure.Exceptions;
using Telegram.Bot.Types;
using SchedulerTelegramBot.Tests.Mocks;
using System;

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
        [Fact]
        public async Task ExecuteCommandAsync_ShouldWork_CommandMatches()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupRepoToContainChat();

            SetupRepoUserIsAdmin();

            Update update = GetUpdateWithMatchingCommand();
            //Act
            var actual = await _sut.ExecuteCommandIfMatched(update);
            //Assert
            AssertMessageBeenSend();
            AssertCommandMatched(actual);
        }
        [Fact]
        public async Task ExecuteCommandAsync_ShouldFail_CommandDontMatch()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupRepoToContainChat();

            SetupRepoUserIsAdmin();

            Update update = GetUpdateWithNotMatchingCommand();
            //Act
            var actual = await _sut.ExecuteCommandIfMatched(update);
            //Assert
            AssertMessageNotBeenSend();
            AssertCommandNotMatched(actual);
        }
        [Fact]
        public async Task ExecuteCommandAsync_ShouldWork_UserIsAdmin()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupRepoToContainChat();

            SetupRepoUserIsAdmin();

            Update update = GetUpdateWithMatchingCommand();
            //Act
            var actual = await _sut.ExecuteCommandIfMatched(update);
            //Assert
            AssertMessageBeenSend();
            AssertCommandMatched(actual);
        }
        [Fact]
        public async Task ExecuteCommandAsync_ShouldFail_UserNotAdmin()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupRepoToContainChat();

            SetupRepoUserIsNotAdmin();

            Update update = GetUpdateWithMatchingCommand();

            //Act
            var actual = await _sut.ExecuteCommandIfMatched(update);
            //Assert
            //permission denied message
            AssertMessageBeenSend();
            AssertCommandNotMatched(actual);
        }
        private void SetupRepoUserIsAdmin()
        {
            _repoMock.Setup(x => x.GetAdminIdOfChat(It.IsAny<string>())).Returns(AdminId);
        }
        private void SetupRepoUserIsNotAdmin()
        {
            _repoMock.Setup(x => x.GetAdminIdOfChat(It.IsAny<string>())).Returns("not" + AdminId);
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

            SetupRepoUserIsAdmin();

            Update update = GetUpdateWithMatchingCommand();

            //Act
            var actual = await _sut.ExecuteCommandIfMatched(update);
            //Assert
            AssertMessageBeenSend();
            AssertCommandMatched(actual);
        }
        [Fact]
        public async Task ExecuteCommandAsync_ShouldFail_ChatNotInSystem()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupRepoToNotContainChat();

            SetupRepoUserIsAdmin();

            Update update = GetUpdateWithMatchingCommand();

            //Act
            await _sut.ExecuteCommandIfMatched(update);
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
        private Update GetUpdateWithMatchingCommand()
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
                        Id = int.Parse(AdminId)
                    }
                }
            };
        }
        private Update GetUpdateWithNotMatchingCommand()
        {
            return new Update
            {
                Message = new Message
                {
                    Text = "/notdeleteChat",
                    Chat = new Chat
                    {
                        Id = long.Parse(TestChatId)
                    },
                    From = new User
                    {
                        Id = int.Parse(AdminId)
                    }
                }
            };
        }
    }
}
