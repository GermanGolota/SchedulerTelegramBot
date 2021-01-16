using Microsoft.Extensions.DependencyInjection;
using SchedulerTelegramBot.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            AddCommandComponents(services);

            services.AddScoped<CommandsContainer>();

            return services;
        }

        private static void AddCommandComponents(IServiceCollection services)
        {
            List<Type> commandTypes = GetCommandTypes();

            foreach (var commandType in commandTypes)
            {

                services.AddScoped(commandType);

                Assembly assembly = Assembly.GetExecutingAssembly();
                Type matcher = assembly.GetIMatcherFor(commandType);
                Type matcherImpl = assembly.GetMatcherImplementationFor(commandType);
                services.AddScoped(matcher, matcherImpl);

                Type contoller = typeof(CommandController<>);
                contoller.MakeGenericType(commandType);
                services.AddScoped(contoller);
            }
        }
        private static List<Type> GetCommandTypes()
        {
            return Assembly.GetExecutingAssembly().GetTypesThatImplement(typeof(ICommand)).ToList();
        }
    }
}
