using Microsoft.Extensions.DependencyInjection;
using SchedulerTelegramBot.Client;
using WebAPI.Commands;

namespace WebAPI.Extensions
{
    public static class TelegramExtensions
    {
        public static IServiceCollection AddTelegramClient(this IServiceCollection services)
        {
            services.AddSingleton<ITelegramBotClientFactory, TelegramBotClientFactory>();

            services.AddSingleton<ITelegramClientAdapter, TelegramClientAdapter>();

            return services;
        }
        public static IServiceCollection AddTelegramCommands(this IServiceCollection services)
        {
            services.AddScoped<StartCommand>();
            services.AddScoped<SetupCommand>();

            services.AddScoped<MessageRepliesContainer>();

            return services;
        }
    }
}
