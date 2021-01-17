using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebAPI.Commands
{
    //needed to avoid using generic type during DI
    public interface ICommandController
    {
        Task<CommandMatchResult> CheckCommand(Update update);
    }
}