using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SchedulerTelegramBot.Client
{
    public class TelegramClientAdapter : ITelegramClientAdapter
    {
        private readonly Lazy<Task<ITelegramBotClient>> telegramClient;

        public TelegramClientAdapter(ITelegramBotClientFactory clientFactory, IConfiguration config)
        {
            string token = config.GetValue<string>("Token");
            string webhook = config.GetValue<string>("Webhook");

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

        public async Task<FileStream> GetFileStreamFromId(string fileId)
        {
            var client = await telegramClient.Value;
            var output = await client.DownloadFileAsync(fileId) as FileStream;
            return output;
        }
    }
}
