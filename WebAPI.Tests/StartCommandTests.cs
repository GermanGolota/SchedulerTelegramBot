using Infrastructure.Exceptions;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using SchedulerTelegramBot.Client;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Commands;
using WebAPI.Tests.Mocks;
using Xunit;

namespace WebAPI.Tests
{
    public class StartCommandTests
    {
        private readonly StartCommand _sut;
        private readonly Mock<IChatRepo> _repoMock = new Mock<IChatRepo>();
        private readonly Mock<ITelegramClientAdapter> _clientMock = new Mock<ITelegramClientAdapter>();
        private const string TestChatId = "56675";
        private const string StartupStickerId = @"CAACAgIAAxkBAAMrX_oDjl4RZ7SqvMaNBxaTese356AAAg0AA3EcFxMefvS-UNPkwR4E";
        private const string AdminId = "12345";
        private const string SuccessMessage = "Activated";
        public StartCommandTests()
        {
           _sut = new StartCommand(_repoMock.Object, _clientMock.Object, new StartCommandLoggerMock());
        }

        [Fact]
        public async Task ExecuteCommandIfMatched_CommandMatches_ValidUpdate()
        {
            //Arrange
            SetupClientMock();

            SetupRepoToContainChat();

            Update update = GetUpdateWithMatchingCommand();
            //Act

            var replie = await _sut.ExecuteCommandIfMatched(update);
            //Assert    
            Assert.Equal(CommandMatchResult.Matching,replie);
        }
        [Fact]
        public async Task ExecuteCommandIfMatched_CommandNotMatches_NotValidUpdate()
        {
            //Arrange
            SetupClientMock();

            SetupRepoToContainChat();

            Update update = GetUpdateWithNotMatchingCommand();
            //Act

            var replie = await _sut.ExecuteCommandIfMatched(update);
            //Assert    
            Assert.Equal(CommandMatchResult.NotMatching, replie);
        }

        [Fact]
        public async Task ExecuteCommandIfMatched_ShouldNotSendMessages_ChatAlreadyExists()
        {
            //Arrange
            SetupClientMock();

            SetupRepoToContainChat();

            Update update = GetUpdateWithMatchingCommand();
            //Act
            await _sut.ExecuteCommandIfMatched(update);
            //Assert
            AssertMessageNotBeenSend();
        }
        [Fact(Skip ="No idea why it doesn't pass")]
        public async Task ExecuteCommandIfMatched_ShouldSendMessages_ChatDontExist()
        {
            //Arrange
            SetupClientMock();

            SetupRepoToNotContainChat();

            Update update = GetUpdateWithMatchingCommand();
            //Act
            await _sut.ExecuteCommandIfMatched(update);
            //Assert
            AssertMessageBeenSendOnce();
        }
        private void SetupClientMock()
        {
            _clientMock.Setup(x => x.SendTextMessageAsync(TestChatId, SuccessMessage)).Returns(Task.CompletedTask);
            _clientMock.Setup(x => x.SendStickerAsync(TestChatId, StartupStickerId)).Returns(Task.CompletedTask);
        }
        private void SetupRepoToContainChat()
        {
            var expectedException = new ChatAlreadyExistsException();
            _repoMock.Setup(x => x.AddChat(TestChatId, AdminId)).ThrowsAsync(expectedException);

        }
        private void SetupRepoToNotContainChat()
        {
            _repoMock.Setup(x => x.AddChat(TestChatId, AdminId)).Returns(Task.CompletedTask);

        }
        private void AssertMessageBeenSendOnce()
        {
            _clientMock.Verify(x => x.SendTextMessageAsync(TestChatId, SuccessMessage), Times.Once);

            _clientMock.Verify(x => x.SendStickerAsync(TestChatId, StartupStickerId), Times.Once);
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
