using Microsoft.Extensions.DependencyInjection;
using SchedulerTelegramBot.Client;
using WebAPI.Commands;
using WebAPI.Commands.Verifiers;

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
            services.AddScoped<DeleteChatCommand>();
            services.AddScoped<DeleteScheduleCommand>();

            services.AddScoped<IMatcher<StartCommand>, StartCommandMatcher>();
            services.AddScoped<IMatcher<SetupCommand>, SetupCommandMatcher>();
            services.AddScoped<IMatcher<DeleteChatCommand>, DeleteChatCommandMatcher>();
            services.AddScoped<IMatcher<DeleteScheduleCommand>, DeleteScheduleCommandMatcher>();

            services.AddScoped<MessageRepliesContainer>();

            return services;
        }
    }
}
