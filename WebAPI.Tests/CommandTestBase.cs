﻿using Moq;
using SchedulerTelegramBot.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace SchedulerTelegramBot.Tests
{
    
    public class CommandTestBase
    {
        protected readonly Mock<ITelegramClientAdapter> _clientMock = new Mock<ITelegramClientAdapter>();
        protected void SetupClientMock()
        {
            _clientMock.Setup(x => x.SendTextMessageAsync(It.IsAny<ChatId>(), It.IsAny<string>()));
            _clientMock.Setup(x => x.SendStickerAsync(It.IsAny<ChatId>(), It.IsAny<string>()));
        }
    }

   
}