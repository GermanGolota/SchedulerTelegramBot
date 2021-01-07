using System.Threading.Tasks;
using Telegram.Bot;

namespace SchedulerTelegramBot.Client
{
    public interface ITelegramBotClientFactory
    {
        Task<ITelegramBotClient> CreateClient(string token, string webhookUrl);
    }
}