using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Client;
using WebAPI.Commands.Verifiers;

namespace WebAPI.Commands.Verifiers
{
    public class AdminCommandMatcherBehaviour<T> : AdminCommandMatcherBase<T> where T : ICommand
    {
        private readonly ITelegramClientAdapter _client;

        public string commandName { get; init; }
        public AdminCommandMatcherBehaviour(IChatRepo repo, ITelegramClientAdapter client):base(repo)
        {
            this._client = client;
        }

        public override async Task<bool> IsMatching(Update update)
        {
            if (UpdateIsCommand(update))
            {
                var message = update.Message;
                string messageText = message.Text ?? "";
                if (FirstWordMatchesCommandName(messageText, commandName))
                {
                    var chatId = message.Chat.Id.ToString();
                    string userId = message.From.Id.ToString();
                    if (!UserIsAdminInChat(userId, chatId))
                    {
                        await _client.SendTextMessageAsync(chatId, StandardMessages.PermissionDenied);
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }
    }

}
