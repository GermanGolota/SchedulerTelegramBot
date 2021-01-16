using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebAPI.Commands
{
    public class CommandsContainer
    {
        private readonly List<ICommand> messageReplies = new List<ICommand>();
        public CommandsContainer(IServiceProvider provider)
        {
            IEnumerable<Type> commandTypes = GetAllChildTypes(typeof(ICommand));
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
        private IEnumerable<Type> GetAllChildTypes(Type parentClass)
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(x=>!x.IsInterface&&x.GetInterfaces().Contains(parentClass));
        }
    }
}
