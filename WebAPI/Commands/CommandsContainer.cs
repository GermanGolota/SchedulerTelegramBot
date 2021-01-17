using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebAPI.Extensions;

namespace WebAPI.Commands
{
    public class CommandsContainer
    {
        private readonly List<ICommand> commands = new List<ICommand>();
        public CommandsContainer(IServiceProvider provider)
        {
            IEnumerable<Type> commandTypes = GetCommandTypes();
            using (var scope = provider.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                foreach (var commandType in commandTypes)
                {
                    ICommand command = (ICommand)serviceProvider.GetRequiredService(commandType);
                    commands.Add(command);
                }
            }
        }
        public IReadOnlyList<ICommand> GetCommands()
        {
            return commands.AsReadOnly();
        }
        private IEnumerable<Type> GetCommandTypes()
        {
            return Assembly.GetExecutingAssembly().GetAllCommands();
        }
    }
}
