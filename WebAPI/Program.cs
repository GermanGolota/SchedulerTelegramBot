using Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
                var client = provider.GetService<ITelegramClient>();
                var config = provider.GetService<IConfiguration>();
                string useLongPulling = config["UseLongPulling"] ?? "false";
                await client.BootUpClient();
                if (useLongPulling.Equals("true"))
                {
                    await UseLongPullingInNewThread(host);
                }
                else
                {
                    string webhook = config["Webhook"];
                    await client.SetupWebhook(webhook);
                }
            }
        }
        private static async Task UseLongPullingInNewThread(IHost host)
        {
            Task.Run(() => GetUpdatesManually(host));
        }
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

                var logger = provider.GetRequiredService<ILogger<Program>>();

                while (true)
                {
                    try
                    {
                        var updates = await client.GetUpdatesAsync(offset: lastUpdateId + 1);

                        foreach (var update in updates)
                        {
                            try
                            {
                                await controller.Update(update);
                                logger.LogInformation("Send message in responce to update");
                            }
                            catch (Exception exc)
                            {
                                logger.LogError(exc, "Something went wrong while sending reply");
                            }
                            lastUpdateId = update.Id;
                        }

                        await Task.Delay(1000);
                    }
                    catch (Exception exc)
                    {
                        logger.LogError(exc, "Something went wrong while receiving updates");
                    }
                }
            }
        }
    }
}
