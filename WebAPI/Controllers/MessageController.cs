using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Commands;
using WebAPI.Commands.Verifiers;

namespace SchedulerTelegramBot.Controllers
{
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly CommandsContainer _container;
        private readonly IServiceProvider _provider;

        public MessageController(CommandsContainer container, IServiceProvider provider)
        {
            this._container = container;
            this._provider = provider;
        }
        [HttpPost("api/message/update")]
        public async Task<IActionResult> Update([FromBody] Update update)
        {
            using (var scope = _provider.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var commands = _container.GetCommands();

                foreach (ICommand command in commands)
                {
                    ICommandController controller = GetCommandController(command, provider);
                    CommandMatchResult result =  await controller.CheckCommand(update);
                    if (result.Equals(CommandMatchResult.Matching))
                    {
                        break;
                    }
                }
            }
            return Ok();
        }

        private ICommandController GetCommandController(ICommand command, IServiceProvider provider)
        {
            Type commandType = command.GetType();
            Type controllerType = typeof(CommandController<>);
            controllerType = controllerType.MakeGenericType(commandType);
             return provider.GetRequiredService(controllerType) as ICommandController;
        }
    }
}
