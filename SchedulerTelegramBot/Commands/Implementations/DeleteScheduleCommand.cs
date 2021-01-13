using Infrastructure.Exceptions;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using SchedulerTelegramBot.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Commands.Verifiers;
using WebAPI.Jobs;

namespace WebAPI.Commands
{
    public class DeleteScheduleCommand : MessageReplyBase
    {
        private readonly IMatcher<DeleteScheduleCommand> _matcher;
        private readonly ITelegramClientAdapter _client;
        private readonly IJobManager _jobs;
        private readonly ILogger<DeleteScheduleCommand> _logger;

        public DeleteScheduleCommand(IMatcher<DeleteScheduleCommand> matcher, ITelegramClientAdapter client, IJobManager jobs,
            ILogger<DeleteScheduleCommand> logger)
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
            string chatId = update.Message.Chat.Id.ToString();
            try
            {
                await _jobs.DeleteJobsFromChat(chatId);
                await _client.SendTextMessageAsync(chatId, StandardMessages.ScheduleDeletionSuccess);
            }
            catch (DataAccessException exc)
            {
                await _client.SendTextMessageAsync(chatId, exc.Message);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "Were not able to delete schedule");
                throw;
            }
        }
    }
}
