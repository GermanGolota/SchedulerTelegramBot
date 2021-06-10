using Moq;
using Telegram.Bot.Types;
using WebAPI.Client;

namespace SchedulerTelegramBot.Tests
{
    public class CommandTestBase
    {
        protected readonly Mock<ITelegramClient> _clientMock = new Mock<ITelegramClient>();

        protected const string TestChatId = "56675";

        protected const string UserId = "12345";
        protected void SetupMessageSendingMock()
        {
            _clientMock.Setup(x => x.SendTextMessageAsync(It.IsAny<ChatId>(), It.IsAny<string>()));
            _clientMock.Setup(x => x.SendStickerAsync(It.IsAny<ChatId>(), It.IsAny<string>()));
        }

    }
}
