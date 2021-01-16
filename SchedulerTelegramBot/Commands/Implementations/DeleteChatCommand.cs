using Infrastructure.Repositories;
using SchedulerTelegramBot.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;
using WebAPI.Commands.Verifiers;

namespace WebAPI.Commands
{
    public class DeleteChatCommand : ICommand
    {
        private readonly ITelegramClientAdapter _client;
        private readonly IChatRepo _repo;
        private readonly ILogger<DeleteChatCommand> _logger;

        public DeleteChatCommand(ITelegramClientAdapter client,
            IChatRepo repo, ILogger<DeleteChatCommand> logger)
        {
            this._client = client;
            this._repo = repo;
            this._logger = logger;
        }

        public async Task Execute(Update update)
        {
            string chatIdToBeDeleted = update.Message.Chat.Id.ToString();
            try
            {
                await _repo.DeleteChat(chatIdToBeDeleted);

                await _client.SendTextMessageAsync(chatIdToBeDeleted, StandardMessages.ChatDeletionSuccess);
            }
            catch (DataAccessException exc)
            {
                await _client.SendTextMessageAsync(chatIdToBeDeleted, exc.Message);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "Were not able to delete chat");
            }
        }
    }
}
