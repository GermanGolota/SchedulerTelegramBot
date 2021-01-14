using Moq;
using SchedulerTelegramBot.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Commands;
using Xunit;

namespace SchedulerTelegramBot.Tests
{
    
    public class CommandMatcherTestBase
    {
        protected readonly Mock<ITelegramClientAdapter> _clientMock = new Mock<ITelegramClientAdapter>();

        protected const string TestChatId = "56675";

        protected const string UserId = "12345";
        protected void SetupMessageSendingMock()
        {
            _clientMock.Setup(x => x.SendTextMessageAsync(It.IsAny<ChatId>(), It.IsAny<string>()));
            _clientMock.Setup(x => x.SendStickerAsync(It.IsAny<ChatId>(), It.IsAny<string>()));
        }

        protected void AssertCommandMatched(bool actual)
        {
            Assert.True(actual);
        }
        protected void AssertCommandNotMatched(bool actual)
        {
            Assert.False(actual);
        }
    }
}
