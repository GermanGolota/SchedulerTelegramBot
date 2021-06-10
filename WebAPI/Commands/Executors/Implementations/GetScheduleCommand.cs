using Infrastructure.Exceptions;
using Infrastructure.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Client;

namespace WebAPI.Commands
{
    public class GetScheduleCommand : ICommand
    {
        private readonly IChatRepo _repo;
        private readonly ITelegramClient _client;

        public GetScheduleCommand(IChatRepo repo, ITelegramClient client)
        {
            this._repo = repo;
            this._client = client;
        }
        public async Task Execute(Update update)
        {
            string chatId = update.Message.Chat.Id.ToString();

            try
            {
                var schedule = await _repo.GetScheduleForChat(chatId);
                string content = JsonConvert.SerializeObject(schedule);
                string fileName = schedule.Name+"Schedule";
                await _client.SendTextFileAsync(chatId, content, fileName);
            }
            catch(DataAccessException ex)
            {
                await _client.SendTextMessageAsync(chatId, ex.Message);
            }
        }
    }
}
