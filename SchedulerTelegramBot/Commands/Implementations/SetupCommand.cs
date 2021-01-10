﻿using Infrastructure.Repositories;
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

namespace WebAPI.Commands
{
    public class SetupCommand : AdminCommandBase
    {
        private readonly ITelegramClientAdapter _client;
        private readonly IJobManager _jobs;
        private readonly ILogger<SetupCommand> _logger;

        public SetupCommand(IChatRepo repo, ITelegramClientAdapter client, IJobManager jobs,
            ILogger<SetupCommand> logger) : base(repo)
        {
            this._client = client;
            this._jobs = jobs;
            this._logger = logger;
        }
        public override string CommandName => "setup";

        protected override async Task<bool> CommandMatches(Update update)
        {
            if (UpdateIsCommand(update))
            {
                var message = update.Message;
                string messageCaption = message.Caption;
                if (FirstWordMatchesCommandName(messageCaption))
                {
                    var chatId = message.Chat.Id.ToString();
                    string userId = message.From.Id.ToString();
                    if (!UserIsAdminInChat(userId, chatId))
                    {
                        await _client.SendTextMessageAsync(chatId, "You don't have permission to do that");
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        protected override async Task ExecuteCommandAsync(Update update)
        {
            var message = update.Message;

            var chatId = message.Chat.Id.ToString();

            var document = message.Document;
            if (document is null)
            {
                await _client.SendTextMessageAsync(chatId, "Please attach a file");
                return;
            }

            var docId = document.FileId;

            string fileContent = await GetFileContent(docId);

            try
            {
                var model = JsonConvert.DeserializeObject<ScheduleModel>(fileContent);
                await SetupJobs(model, chatId);
            }
            catch (JsonException)
            {
                await _client.SendTextMessageAsync(chatId, "Data in the file is not valid");
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

            await _client.SendTextMessageAsync(chatId, "Schedule have been succesfully set");

        }
    }
}
