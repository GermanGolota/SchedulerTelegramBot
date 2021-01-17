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
        public SetupCommandMatcher(IChatRepo repo, ITelegramClientAdapter client) : base(repo)
        {
            this._client = client;
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

                    if(messageText!=null&&FirstWordMatchesCommandName(messageText, commandName))
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
