using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebAPI.Commands
{
    public interface ICommand
    {
        Task Execute(Update update);
    }
}
