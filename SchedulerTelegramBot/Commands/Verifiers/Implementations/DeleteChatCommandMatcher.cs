using Infrastructure.Repositories;
using SchedulerTelegramBot.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebAPI.Commands.Verifiers
{
    public class DeleteChatCommandMatcher : AdminCommandMatcherBase<DeleteChatCommand>
    {
        private readonly ITelegramClientAdapter _client;

        public DeleteChatCommandMatcher(DeleteChatCommand command, IChatRepo repo, ITelegramClientAdapter client) :base(command, repo)
        {
            this._client = client;
        }
        public override async Task<bool> IsMatching(Update update)
        {
            if (UpdateIsCommand(update))
            {
                var message = update.Message;
                string messageText = message.Text;
                if (FirstWordMatchesCommandName(messageText))
                {
                    string chatId = message.Chat.Id.ToString();
                    string userId = message.From.Id.ToString();
                    if (!UserIsAdminInChat(userId, chatId))
                    {
                        await _client.SendTextMessageAsync(chatId,
                            StandardMessages.PermissionDenied);
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
