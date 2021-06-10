using Infrastructure.DTOs;
using Infrastructure.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Client;
using WebAPI.Jobs;

namespace WebAPI.Commands
{
    public class AddAlertCommand : ICommand
    {
        private readonly ITelegramClient _client;
        private readonly IUpdateManager _updateHelper;
        private readonly IJobManager _jobs;

        public AddAlertCommand(ITelegramClient client, IUpdateManager updater, IJobManager jobs)
        {
            this._client = client;
            this._updateHelper = updater;
            this._jobs = jobs;
        }
        public async Task Execute(Update update)
        {
            var chatId = update.Message.Chat.Id;
            string fileContent;
            try
            {
                fileContent = await _updateHelper.GetFileContentsFrom(update);
            }
            catch (DataAccessException ex)
            {
                await _client.SendTextMessageAsync(chatId, ex.Message);
                return;
            }
            ScheduleUpdateModel model;
            try
            {
                model = JsonConvert.DeserializeObject<ScheduleUpdateModel>(fileContent);
            }
            catch (JsonException)
            {
                await _client.SendTextMessageAsync(chatId, StandardMessages.BadFileData);
                return;
            }

            try
            {
                await _jobs.AddJobsToChatWithExistingSchedule(chatId, model);
            }
            catch(DataAccessException ex)
            {
                await _client.SendTextMessageAsync(chatId, ex.Message);
            }

            await _client.SendTextMessageAsync(chatId, StandardMessages.AddedAlertsSuccess);
        }

    }
}
