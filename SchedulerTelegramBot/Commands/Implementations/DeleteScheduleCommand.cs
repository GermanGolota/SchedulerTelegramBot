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
    public class DeleteScheduleCommand : ICommand
    {
        private readonly ITelegramClientAdapter _client;
        private readonly IJobManager _jobs;
        private readonly ILogger<DeleteScheduleCommand> _logger;

        public DeleteScheduleCommand(ITelegramClientAdapter client, IJobManager jobs,
            ILogger<DeleteScheduleCommand> logger)
        {
            this._client = client;
            this._jobs = jobs;
            this._logger = logger;
        }

        public async Task Execute(Update update)
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
