using Infrastructure.Repositories;
using SchedulerTelegramBot.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;

namespace WebAPI.Commands
{
    public class DeleteChatCommand : AdminCommandBase
    {
        private readonly ITelegramClientAdapter _client;
        private readonly IChatRepo _repo;
        private readonly ILogger<DeleteChatCommand> _logger;

        public string ChatIdToBeDeleted { get; private set; }

        public DeleteChatCommand(ITelegramClientAdapter client, IChatRepo repo, ILogger<DeleteChatCommand> logger):base(repo)
        {
            this._client = client;
            this._repo = repo;
            this._logger = logger;
        }
        public override string CommandName => "deleteChat";

        protected override async Task<bool> CommandMatches(Update update)
        {
            if (UpdateIsCommand(update))
            {
                var message = update.Message;
                string messageText = message.Text;
                if (FirstWordMatchesCommandName(messageText))
                {
                    ChatIdToBeDeleted = message.Chat.Id.ToString();
                    string userId = message.From.Id.ToString();
                    if (!UserIsAdminInChat(userId, ChatIdToBeDeleted))
                    {
                        await _client.SendTextMessageAsync(ChatIdToBeDeleted, "You don't have permission to do that");
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        protected override async Task ExecuteCommandAsync(Update update)
        {
            try
            {
                await _repo.DeleteChat(ChatIdToBeDeleted);
            }
            catch(ChatDontExistException)
            {
                await _client.SendTextMessageAsync(ChatIdToBeDeleted, "This chat is not being traced");
            }
            catch(Exception exc)
            {
                _logger.LogError(exc, "Were not able to delete chat");
            }
            await _client.SendTextMessageAsync(ChatIdToBeDeleted, "Successfully deleted chat");
        }
    }
}
