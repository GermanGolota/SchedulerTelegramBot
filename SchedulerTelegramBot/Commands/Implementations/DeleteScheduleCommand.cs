using Infrastructure.Exceptions;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using SchedulerTelegramBot.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Jobs;

namespace WebAPI.Commands
{
    public class DeleteScheduleCommand : AdminCommandBase
    {
        private readonly ITelegramClientAdapter _client;
        private readonly IJobManager _jobs;
        private readonly ILogger<DeleteScheduleCommand> _logger;

        public DeleteScheduleCommand(IChatRepo repo, ITelegramClientAdapter client, IJobManager jobs, 
            ILogger<DeleteScheduleCommand> logger) : base(repo)
        {
            this._client = client;
            this._jobs = jobs;
            this._logger = logger;
        }
        public override string CommandName => "deleteSchedule";

        protected override async Task<bool> CommandMatches(Update update)
        {
            if (UpdateIsCommand(update))
            {
                var message = update.Message;
                string messageText = message.Text;
                if (FirstWordMatchesCommandName(messageText))
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
            string chatId = update.Message.Chat.Id.ToString();
            try
            {
                await _jobs.DeleteJobsFromChat(chatId);
                await _client.SendTextMessageAsync(chatId, "Succesfully deleted schedule");
            }
            catch(DataAccessException exc)
            {
                await _client.SendTextMessageAsync(chatId, exc.Message);
            }
        }
    }
}
