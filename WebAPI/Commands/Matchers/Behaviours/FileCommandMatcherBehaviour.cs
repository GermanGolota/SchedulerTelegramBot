using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Client;

namespace WebAPI.Commands.Verifiers
{
    public class FileAdminCommandMatcherBehaviour<T> : AdminCommandMatcherBase<T> where T : ICommand
    {
        private readonly ITelegramClientAdapter _client;
        private string commandName { get; init; }

        public FileAdminCommandMatcherBehaviour(ITelegramClientAdapter client, IChatRepo repo, string commandName):base(repo)
        {
            this._client = client;
            this.commandName = commandName;
        }
        public override async Task<bool> IsMatching(Update update)
        {
            if (UpdateIsCommand(update))
            {
                var message = update.Message;
                var chatId = message.Chat.Id.ToString();
                if (message.Caption is null)
                {
                    string messageText = message.Text;

                    if (messageText != null && FirstWordMatchesCommandName(messageText, commandName))
                    {
                        await _client.SendTextMessageAsync(chatId, StandardMessages.NoFileAttached);
                    }

                    return false;
                }
                string messageCaption = message.Caption;
                if (FirstWordMatchesCommandName(messageCaption, commandName))
                {
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
