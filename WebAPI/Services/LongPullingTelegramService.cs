using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SchedulerTelegramBot.Controllers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using WebAPI.Commands;

namespace WebAPI.Services
{
    public class LongPullingTelegramService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public LongPullingTelegramService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var provider = scope.ServiceProvider;
                CommandsContainer commandsContainer = provider.GetService<CommandsContainer>();
                var controller = new MessageController(commandsContainer, provider);
                var config = provider.GetService<IConfiguration>();
                string token = config.GetValue<string>("Token");
                TelegramBotClient client = new(token);
                await client.DeleteWebhookAsync();

                var initialUpdates = await client.GetUpdatesAsync(cancellationToken: stoppingToken);

                var lastUpdate = initialUpdates.LastOrDefault();

                int lastUpdateId = lastUpdate?.Id ?? 0;

                var logger = provider.GetRequiredService<ILogger<LongPullingTelegramService>>();

                while (stoppingToken.IsCancellationRequested == false)
                {
                    try
                    {
                        var updates = await client.GetUpdatesAsync(offset: lastUpdateId + 1, cancellationToken: stoppingToken);

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

                        await Task.Delay(1000, stoppingToken);
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
