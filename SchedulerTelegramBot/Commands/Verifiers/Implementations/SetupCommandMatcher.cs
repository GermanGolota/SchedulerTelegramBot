using Infrastructure.Repositories;
using SchedulerTelegramBot.Client;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebAPI.Commands.Verifiers
{
    public class SetupCommandMatcher : AdminCommandMatcherBase<SetupCommand>
    {
        private readonly ITelegramClientAdapter _client;
        private string commandName = "setup";
        public SetupCommandMatcher(IChatRepo repo, ITelegramClientAdapter client, SetupCommand command) : base(command, repo)
        {
            this._client = client;
        }
        public override async Task<bool> IsMatching(Update update)
        {
            if (UpdateIsCommand(update))
            {
                var message = update.Message;
                string messageCaption = message.Caption ?? "";
                if (FirstWordMatchesCommandName(messageCaption, commandName))
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
