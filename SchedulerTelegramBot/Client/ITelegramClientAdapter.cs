﻿using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace SchedulerTelegramBot.Client
{
    public interface ITelegramClientAdapter
    {
        Task BootUpClient();
        Task SendTextMessageAsync(ChatId chat, string message);
    }
}