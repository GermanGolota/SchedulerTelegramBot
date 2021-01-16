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
            AddCommandExecutors(services);

            AddCommandMatchers(services);

            AddCommandController(services);

            services.AddScoped<CommandsContainer>();

            return services;
        }
        private static void AddCommandExecutors(IServiceCollection services)
        {

            services.AddScoped<StartCommand>();
            services.AddScoped<SetupCommand>();
            services.AddScoped<DeleteChatCommand>();
            services.AddScoped<DeleteScheduleCommand>();
        }
        private static void AddCommandMatchers(IServiceCollection services)
        {
            services.AddScoped<IMatcher<StartCommand>, StartCommandMatcher>();
            services.AddScoped<IMatcher<SetupCommand>, SetupCommandMatcher>();
            services.AddScoped<IMatcher<DeleteChatCommand>, DeleteChatCommandMatcher>();
            services.AddScoped<IMatcher<DeleteScheduleCommand>, DeleteScheduleCommandMatcher>();
        }
        private static void AddCommandController(IServiceCollection services)
        {
            services.AddScoped<CommandController<StartCommand>>();
            services.AddScoped<CommandController<SetupCommand>>();
            services.AddScoped<CommandController<DeleteChatCommand>>();
            services.AddScoped<CommandController<DeleteScheduleCommand>>();
        }
    }
}
