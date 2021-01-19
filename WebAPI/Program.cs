using Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SchedulerTelegramBot.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using WebAPI.Client;
using WebAPI.Commands;

namespace WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            MigrateDatabase(host);

            await InitializeTelegramClient(host);

            host.Run();
        }

        private static void MigrateDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var context = provider.GetRequiredService<SchedulesContext>();
                context.Database.Migrate();
            }
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
            }
#if DEBUG
            Task.Run(() => GetUpdatesManually(host));
#endif
        }
#if DEBUG
        private static async Task GetUpdatesManually(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                CommandsContainer commandsContainer = provider.GetService<CommandsContainer>();
                var controller = new MessageController(commandsContainer, provider);
                var config = provider.GetService<IConfiguration>();
                string token = config.GetValue<string>("Token");
                TelegramBotClient client = new TelegramBotClient(token);
                await client.DeleteWebhookAsync();

                var initialUpdates = await client.GetUpdatesAsync();

                var lastUpdate = initialUpdates.LastOrDefault();

                int lastUpdateId = lastUpdate?.Id ?? 0;

                while (true)
                {
                    var updates = await client.GetUpdatesAsync(offset: lastUpdateId + 1);

                    foreach (var update in updates)
                    {
                        try
                        {
                            await controller.Update(update);
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine(exc.Message);
                        }
                        lastUpdateId = update.Id;
                    }

                    await Task.Delay(1000);
                }
            }
        }
#endif
    }
}
