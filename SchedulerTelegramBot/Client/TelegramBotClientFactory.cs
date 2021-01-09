using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;

namespace SchedulerTelegramBot.Client
{
    public class TelegramBotClientFactory : ITelegramBotClientFactory
    {
        private readonly HttpClient _client;

        public TelegramBotClientFactory(HttpClient client)
        {
            this._client = client;
        }
        public async Task<ITelegramBotClient> CreateClient(string token, string webhookUrl)
        {
            var client = new TelegramBotClient(token, _client);

            await client.DeleteWebhookAsync();

            await client.SetWebhookAsync(webhookUrl);

            return client;
        }
    }
}
