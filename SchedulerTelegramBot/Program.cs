using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SchedulerTelegramBot.Client;
using SchedulerTelegramBot.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using WebAPI.Commands;

namespace SchedulerTelegramBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await InitializeTelegramClient(host);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel();
                    webBuilder.UseStartup<Startup>();
                });
        }
        private static async Task InitializeTelegramClient(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var client = provider.GetService<ITelegramClientAdapter>();
                await client.BootUpClient();
#if DEBUG
                await GetUpdatesManually(provider);
#endif
            }
        }
#if DEBUG
        private static async Task GetUpdatesManually(IServiceProvider provider)
        {
            var config = provider.GetService<IConfiguration>();
            string token = config.GetValue<string>("Token");
            TelegramBotClient client = new TelegramBotClient(token);
            await client.DeleteWebhookAsync();

            var repliesContainer = provider.GetService<MessageRepliesContainer>();

            var controller = new MessageController(repliesContainer);

            var initialUpdates = await client.GetUpdatesAsync();

            var lastUpdate = initialUpdates.LastOrDefault();

            int lastUpdateId = lastUpdate?.Id ?? 0;

            while (true)
            {
                var updates = await client.GetUpdatesAsync(offset: lastUpdateId+1);

                foreach (var update in updates)
                {
                    await controller.Update(update);
                    lastUpdateId = update.Id;
                }

                await Task.Delay(1000);
            }
        }
#endif
    }
}
