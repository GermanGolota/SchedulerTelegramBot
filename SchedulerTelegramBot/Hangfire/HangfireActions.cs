using SchedulerTelegramBot.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebAPI.Hangfire
{
    public class HangfireActions
    {
        private readonly ITelegramClientAdapter _client;

        public HangfireActions(ITelegramClientAdapter client)
        {
            this._client = client;
        }

        public async Task SendAlertMessage(string Message, string ChatId)
        {
            await _client.SendTextMessageAsync(new ChatId(ChatId), Message);
        }
    }
}
