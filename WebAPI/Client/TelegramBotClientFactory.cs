using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;

namespace WebAPI.Client
{
    public class TelegramBotClientFactory : ITelegramBotClientFactory
    {
        private readonly HttpClient _client;

        public TelegramBotClientFactory(HttpClient client)
        {
            this._client = client;
        }
        public async Task<ITelegramBotClient> CreateClient(string token)
        {
            var client = new TelegramBotClient(token, _client);

            return client;
        }
    }
}
