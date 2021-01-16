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
        private readonly List<ICommand> messageReplies = new List<ICommand>();
        public CommandsContainer(IServiceProvider provider)
        {
            IEnumerable<Type> commandTypes = GetCommands();
            using (var scope = provider.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                foreach (var commandType in commandTypes)
                {
                    ICommand command = (ICommand)serviceProvider.GetRequiredService(commandType);
                    messageReplies.Add(command);
                }
            }
        }
        public IReadOnlyList<ICommand> GetMessageReplies()
        {
            return messageReplies.AsReadOnly();
        }
        private IEnumerable<Type> GetCommands()
        {
            return Assembly.GetExecutingAssembly().GetAllCommands();
        }
    }
}
