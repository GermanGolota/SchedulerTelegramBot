using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebAPI.Commands
{
    public interface IUpdateManager
    {
        Task<string> GetFileContentsFrom(Update update);
    }
}