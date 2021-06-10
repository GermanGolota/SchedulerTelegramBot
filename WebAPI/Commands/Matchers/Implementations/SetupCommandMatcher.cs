using Infrastructure.Repositories;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Client;

namespace WebAPI.Commands.Verifiers
{
    public class SetupCommandMatcher : FileAdminCommandMatcherBehaviour<SetupCommand>
    {
        public SetupCommandMatcher(IChatRepo repo, ITelegramClient client) 
            : base(client,repo, CommandNames.Setup)
        {
        }
    }
}
