using Infrastructure.Exceptions;
using Infrastructure.Repositories;
using Moq;
using SchedulerTelegramBot.Tests.Mocks;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Commands;
using WebAPI.Commands.Verifiers;
using Xunit;

namespace SchedulerTelegramBot.Tests
{
    public class StartCommandTests : CommandTestBase
    {
        private readonly StartCommand _sut;
        private readonly Mock<IChatRepo> _repoMock = new Mock<IChatRepo>();
        private readonly Mock<IMatcher<StartCommand>> _matcherMock = new Mock<IMatcher<StartCommand>>();
        private const string StartupStickerId = @"CAACAgIAAxkBAAMrX_oDjl4RZ7SqvMaNBxaTese356AAAg0AA3EcFxMefvS-UNPkwR4E";
        private string SuccessMessage = StandardMessages.ChatRegistration;
        public StartCommandTests()
        {
            _sut = new StartCommand(_matcherMock.Object,
                _repoMock.Object, _clientMock.Object, new LoggerMock<StartCommand>());
        }

        [Fact]
        public async Task ExecuteCommandIfMatched_ShouldNotSendMessages_ChatAlreadyExists()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupMatcherVaildCommand();

            SetupRepoToContainChat();

            Update update = GetUpdate();
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

            SetupMatcherVaildCommand();

            SetupRepoToNotContainChat();

            Update update = GetUpdate();
            //Act
            await _sut.ExecuteCommandIfMatched(update);
            //Assert
            AssertMessageBeenSendOnce();
        }

        private void SetupMatcherVaildCommand()
        {
            _matcherMock.Setup(x => x.IsMatching(It.IsAny<Update>())).ReturnsAsync(true);
        }

        private void SetupRepoToContainChat()
        {
            var expectedException = new ChatAlreadyExistsException();
            _repoMock.Setup(x => x.AddChat(TestChatId, UserId)).ThrowsAsync(expectedException);

        }
        private void SetupRepoToNotContainChat()
        {
            _repoMock.Setup(x => x.AddChat(TestChatId, UserId));

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

        private Update GetUpdate()
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
                        Id = int.Parse(UserId)
                    }
                }
            };
        }
    }
}
