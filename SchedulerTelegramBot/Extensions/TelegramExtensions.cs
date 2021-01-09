using Microsoft.Extensions.DependencyInjection;
using SchedulerTelegramBot.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            services.AddScoped<MessageRepliesContainer>();

            return services;
        }
    }
}
