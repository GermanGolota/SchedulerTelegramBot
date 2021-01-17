using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Client;

namespace WebAPI.Commands
{
    public class CreateScheduleCommand : ICommand
    {
        private readonly ITelegramClientAdapter _client;

        public CreateScheduleCommand(ITelegramClientAdapter client)
        {
            this._client = client;
        }
        public async Task Execute(Update update)
        {
            var chatId = update.Message.Chat.Id;
            await _client.SendTextMessageAsync(chatId, "This feature is not yet implemented");
        }
    }
}
