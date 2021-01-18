using Infrastructure.Repositories;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Client;

namespace WebAPI.Commands.Verifiers
{
    public class SetupCommandMatcher : FileAdminCommandMatcherBehaviour<SetupCommand>
    {
        public SetupCommandMatcher(IChatRepo repo, ITelegramClientAdapter client) 
            : base(client,repo, CommandNames.Setup)
        {
        }
    }
}
