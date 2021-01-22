using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

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

            this.telegramClient = new Lazy<Task<ITelegramBotClient>>(async () =>
            {
                return await clientFactory.CreateClient(token);
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

            string fileLocation = GetRandomLocationForFile();
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
        private string GetRandomLocationForFile()
        {
            string fileBase = _config.GetValue<string>("DownloadFilesLocationBase");
            return fileBase + Guid.NewGuid().ToString() + ".json";
        }
        public async Task SendTextFileAsync(ChatId chat, string fileContent, string fileName)
        {
            var client = await telegramClient.Value;
            string location = GetRandomLocationForFile();
            using (FileStream stream = new FileStream(location, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.Write(fileContent);
                }
            }
            FileStream fs = new FileStream(location, FileMode.Open, FileAccess.Read);
            InputOnlineFile file = new InputOnlineFile(fs, fileName);
            await client.SendDocumentAsync(chat, file);
            fs.Close();
        }

        public async Task SetupWebhook(string webhookUrl)
        {
            var client = await telegramClient.Value;

            await client.DeleteWebhookAsync();

            await client.SetWebhookAsync(webhookUrl);
        }
    }
}
