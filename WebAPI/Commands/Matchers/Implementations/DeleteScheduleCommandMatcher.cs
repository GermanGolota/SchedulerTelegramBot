using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Client;

namespace WebAPI.Commands.Verifiers
{
    public class DeleteScheduleCommandMatcher : AdminCommandMatcherBase<DeleteScheduleCommand>
    {
        private readonly ITelegramClientAdapter _client;

        private string commandName = "deleteSchedule";
        public DeleteScheduleCommandMatcher(IChatRepo repo, ITelegramClientAdapter client):base(repo)
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
