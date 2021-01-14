using Moq;
using SchedulerTelegramBot.Client;
using Telegram.Bot.Types;

namespace SchedulerTelegramBot.Tests
{
    public class CommandTestBase
    {
        protected readonly Mock<ITelegramClientAdapter> _clientMock = new Mock<ITelegramClientAdapter>();

        protected const string TestChatId = "56675";

        protected const string UserId = "12345";
        protected void SetupMessageSendingMock()
        {
            _clientMock.Setup(x => x.SendTextMessageAsync(It.IsAny<ChatId>(), It.IsAny<string>()));
            _clientMock.Setup(x => x.SendStickerAsync(It.IsAny<ChatId>(), It.IsAny<string>()));
        }

    }
}
