using Infrastructure.Exceptions;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using SchedulerTelegramBot.Client;
using SchedulerTelegramBot.Tests;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Commands;
using Xunit;

namespace SchedulerTelegramBot.Tests
{
    public class StartCommandTests:CommandTestBase
    {
        private readonly StartCommand _sut;
        private readonly Mock<IChatRepo> _repoMock = new Mock<IChatRepo>();
        private const string TestChatId = "56675";
        private const string StartupStickerId = @"CAACAgIAAxkBAAMrX_oDjl4RZ7SqvMaNBxaTese356AAAg0AA3EcFxMefvS-UNPkwR4E";
        private const string AdminId = "12345";
        private string SuccessMessage = StandardMessages.ChatRegistration;
        public StartCommandTests()
        {
           _sut = new StartCommand(_repoMock.Object, _clientMock.Object, new LoggerMock<StartCommand>());
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
        [Fact]
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
