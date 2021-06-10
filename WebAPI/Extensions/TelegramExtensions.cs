using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebAPI.Client;
using WebAPI.Commands;

namespace WebAPI.Extensions
{
    public static class TelegramExtensions
    {
        public static IServiceCollection AddTelegramClient(this IServiceCollection services)
        {
            services.AddSingleton<TelegramBotClientFactory>();

            services.AddSingleton<ITelegramClient, TelegramClientAdapter>();

            return services;
        }
        public static IServiceCollection AddTelegramCommands(this IServiceCollection services, Assembly assembly)
        {
            AddCommandComponents(services, assembly);

            services.AddScoped<CommandsContainer>();

            return services;
        }

        private static void AddCommandComponents(IServiceCollection services, Assembly assembly)
        {
            List<Type> commandTypes = GetCommandTypes(assembly);

            foreach (var commandType in commandTypes)
            {
                services.AddScoped(commandType);

                Type matcher = commandType.GetIMatcher();
                Type matcherImpl = assembly.GetMatcherImplementationFor(commandType);
                services.AddScoped(matcher, matcherImpl);

                Type contoller = typeof(CommandController<>);
                contoller.MakeGenericType(commandType);
                services.AddScoped(contoller);
            }
        }
        private static List<Type> GetCommandTypes(Assembly assembly)
        {
            return assembly.GetAllCommands().ToList();
        }
    }
}
