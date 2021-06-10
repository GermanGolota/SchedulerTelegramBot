using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;

namespace WebAPI.Client
{
    public class TelegramBotClientFactory
    {
        private readonly HttpClient _client;

        public TelegramBotClientFactory(HttpClient client)
        {
            this._client = client;
        }

        public Task<ITelegramBotClient> CreateClient(string token)
        {
            var client = new TelegramBotClient(token, _client);
            return Task.FromResult(client as ITelegramBotClient);
        }
    }
}
