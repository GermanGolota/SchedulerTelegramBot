using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace WebAPI.Client
{
    public class TelegramClientAdapter : ITelegramClient
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

            string tempFile = "temp.txt";

            using (FileStream fs = new FileStream(tempFile, FileMode.Create))
            {
                await client.DownloadFileAsync(file.FilePath, fs);
            }
            string fileContent;
            using (StreamReader sr = new StreamReader(new FileStream(tempFile, FileMode.Open)))
            {
                fileContent = sr.ReadToEnd();    
            }

            System.IO.File.Delete(tempFile);

            return fileContent;
        }

        public async Task SendStickerAsync(ChatId chat, string stickerLocation)
        {
            var client = await telegramClient.Value;
            await client.SendStickerAsync(chat, stickerLocation);
        }
        public async Task SendTextFileAsync(ChatId chat, string fileContent, string fileName)
        {
            var client = await telegramClient.Value;
            string tempFile = "temp.txt";
            using (FileStream stream = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.Write(fileContent);
                }
            }
            FileStream fs = new FileStream(tempFile, FileMode.Open, FileAccess.Read);
            InputOnlineFile file = new InputOnlineFile(fs, fileName);
            await client.SendDocumentAsync(chat, file);
            fs.Close();
            System.IO.File.Delete(tempFile);
        }

        public async Task SetupWebhook(string webhookUrl)
        {
            var client = await telegramClient.Value;

            await client.DeleteWebhookAsync();

            await client.SetWebhookAsync(webhookUrl);
        }
    }
}
