using Infrastructure.Repositories;
using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Newtonsoft.Json;
using Infrastructure.DTOs;
using WebAPI.Jobs;
using Microsoft.Extensions.Logging;
using Infrastructure.Exceptions;
using WebAPI.Commands.Verifiers;
using WebAPI.Client;

namespace WebAPI.Commands
{
    public class SetupCommand : ICommand
    {
        private readonly ITelegramClient _client;
        private readonly IJobManager _jobs;
        private readonly ILogger<SetupCommand> _logger;
        private readonly IUpdateManager _updateHelper;

        public SetupCommand(ITelegramClient client, IJobManager jobs,
            ILogger<SetupCommand> logger, IUpdateManager updateHelper)
        {
            this._client = client;
            this._jobs = jobs;
            this._logger = logger;
            this._updateHelper = updateHelper;
        }

        public async Task Execute(Update update)
        {
            string fileContent = await _updateHelper.GetFileContentsFrom(update);

            string chatId = update.Message.Chat.Id.ToString();

            try
            {
                var model = JsonConvert.DeserializeObject<ScheduleModel>(fileContent);
                await SetupJobs(model, chatId);

                await _client.SendTextMessageAsync(chatId, StandardMessages.ScheduleSetSuccess);
            }
            catch (JsonException)
            {
                await _client.SendTextMessageAsync(chatId, StandardMessages.BadFileData);
            }
            catch (DataAccessException exc)
            {
                await _client.SendTextMessageAsync(chatId, exc.Message);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "Were not able to setup jobs");
                throw;
            }
        }
        private async Task SetupJobs(ScheduleModel model, string chatId)
        {
            await _jobs.SetupJobsForChat(model, chatId);
        }
    }
}
