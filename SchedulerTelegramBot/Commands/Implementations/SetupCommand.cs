using Infrastructure.Repositories;
using SchedulerTelegramBot.Client;
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

namespace WebAPI.Commands
{
    public class SetupCommand : CommandBase
    {
        private readonly IMatcher<SetupCommand> _matcher;
        private readonly ITelegramClientAdapter _client;
        private readonly IJobManager _jobs;
        private readonly ILogger<SetupCommand> _logger;

        public SetupCommand(IMatcher<SetupCommand> matcher, ITelegramClientAdapter client, IJobManager jobs,
            ILogger<SetupCommand> logger)
        {
            this._matcher = matcher;
            this._client = client;
            this._jobs = jobs;
            this._logger = logger;
        }

        protected override async Task<bool> CommandMatches(Update update)
        {
            return await _matcher.IsMatching(update);
        }

        protected override async Task ExecuteCommandAsync(Update update)
        {
            var message = update.Message;

            var chatId = message.Chat.Id.ToString();

            var document = message.Document;
            if (document is null)
            {
                await _client.SendTextMessageAsync(chatId, StandardMessages.NoFileAttached);
                return;
            }

            var docId = document.FileId;

            string fileContent = await GetFileContent(docId);

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
        private async Task<string> GetFileContent(string fileId)
        {
            string fileLocation = await _client.DownloadFileFromId(fileId);
            using (FileStream stream = new FileStream(fileLocation, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    string fileContent = sr.ReadToEnd();
                    return fileContent;
                }
            }
        }
        private async Task SetupJobs(ScheduleModel model, string chatId)
        {
            await _jobs.SetupJobsForChat(model, chatId);
        }
    }
}
