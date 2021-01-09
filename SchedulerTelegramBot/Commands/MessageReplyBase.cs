using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebAPI.Commands
{
    public abstract class MessageReplyBase
    {
        public abstract string CommandName { get; }
        public async Task<CommandMatchResult> ExecuteCommandIfMatched(Update update)
        {
            if (CommandMatches(update))
            {
                await ExecuteCommandAsync(update);
                return CommandMatchResult.Matching;
            }
            else
            {
                return CommandMatchResult.NotMatching;
            }
        }
        protected abstract Task ExecuteCommandAsync(Update update);
        protected abstract bool CommandMatches(Update update);
    }
}
