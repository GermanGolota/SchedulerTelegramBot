using Infrastructure.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Commands;
using WebAPI.Commands.Verifiers;
using WebAPI.Jobs;

namespace SchedulerTelegramBot.Tests.CommandTests.Matchers
{
    public class StartCommandMatcherTests:CommandMatcherTestBase
    {
        public IMatcher<StartCommand> _sut;
        private readonly Mock<StartCommand> _commandMock = new Mock<StartCommand>();
        public StartCommandMatcherTests()
        {
            _sut = new StartCommandMatcher(_commandMock.Object);
        }
        [Fact]
        public async Task ExecuteCommandIfMatched_CommandMatches_ValidUpdate()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupRepoToContainChat();

            Update update = GetUpdateWithMatchingCommand();
            //Act

            var actual = await _sut.ExecuteCommandIfMatched(update);
            //Assert    
            AssertCommandMatched(actual);
        }
        [Fact]
        public async Task ExecuteCommandIfMatched_CommandNotMatches_NotValidUpdate()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupRepoToContainChat();

            Update update = GetUpdateWithNotMatchingCommand();
            //Act

            var actual = await _sut.ExecuteCommandIfMatched(update);
            //Assert    
            AssertCommandNotMatched(actual);
        }

        [Fact]
        public async Task ExecuteCommandIfMatched_ShouldNotSendMessages_ChatAlreadyExists()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupRepoToContainChat();

            Update update = GetUpdateWithMatchingCommand();
            //Act
            await _sut.ExecuteCommandIfMatched(update);
            //Assert
            AssertMessageNotBeenSend();
        }
        [Fact]
        public async Task ExecuteCommandIfMatched_ShouldSendMessages_ChatDontExist()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupRepoToNotContainChat();

            Update update = GetUpdateWithMatchingCommand();
            //Act
            await _sut.ExecuteCommandIfMatched(update);
            //Assert
            AssertMessageBeenSendOnce();
        }
        private void SetupRepoToContainChat()
        {
            var expectedException = new ChatAlreadyExistsException();
            _repoMock.Setup(x => x.AddChat(TestChatId, AdminId)).ThrowsAsync(expectedException);

        }
        private void SetupRepoToNotContainChat()
        {
            _repoMock.Setup(x => x.AddChat(TestChatId, AdminId));

        }
        private void AssertMessageBeenSendOnce()
        {
            _clientMock.Verify(x => x.SendTextMessageAsync(It.IsAny<ChatId>(), It.IsAny<string>()), Times.Once);

            _clientMock.Verify(x => x.SendStickerAsync(It.IsAny<ChatId>(), It.IsAny<string>()), Times.Once);
        }
        private void AssertMessageNotBeenSend()
        {
            _clientMock.Verify(x => x.SendStickerAsync(TestChatId, StartupStickerId), Times.Never);
            _clientMock.Verify(x => x.SendTextMessageAsync(TestChatId, SuccessMessage), Times.Never);
        }

        private Update GetUpdateWithMatchingCommand()
        {
            return new Update
            {
                Message = new Message
                {
                    Text = "/start",
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
                    Text = "/notStart",
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
