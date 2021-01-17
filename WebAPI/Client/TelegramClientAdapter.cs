using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WebAPI.Client
{
    public class TelegramClientAdapter : ITelegramClientAdapter
    {
        private readonly Lazy<Task<ITelegramBotClient>> telegramClient;
        private readonly IConfiguration _config;

        public TelegramClientAdapter(ITelegramBotClientFactory clientFactory, IConfiguration config)
        {

            _config = config;
            string token = _config.GetValue<string>("Token");
            string webhook = _config.GetValue<string>("Webhook");

            this.telegramClient = new Lazy<Task<ITelegramBotClient>>(async () =>
            {
                return await clientFactory.CreateClient(token, webhook);
            });
        }

        public async Task SendTextMessageAsync(ChatId chat, string message)
        {
            var client = await telegramClient.Value;
            await client.SendTextMessageAsync(chat, message);
        }
        //Webhook needs to be setup before receiving any messages
        public async Task BootUpClient()
        {
            await telegramClient.Value;
        }
        //returns location to which the file were stored
        public async Task<string> DownloadFileFromId(string fileId)
        {
            var client = await telegramClient.Value;
            var file = await client.GetFileAsync(fileId);
            string fileBase = _config.GetValue<string>("DownloadFilesLocationBase");
            string fileLocation = fileBase + Guid.NewGuid().ToString()+".json";
            FileStream fs = new FileStream(fileLocation, FileMode.Create);
            await client.DownloadFileAsync(file.FilePath, fs);
            fs.Close();
            return fileLocation;
        }

        public async Task SendStickerAsync(ChatId chat, string stickerLocation)
        {
            var client = await telegramClient.Value;
            await client.SendStickerAsync(chat, stickerLocation);
        }
    }
}
