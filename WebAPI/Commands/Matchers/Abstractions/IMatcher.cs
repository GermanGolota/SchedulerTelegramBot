using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebAPI.Commands.Verifiers
{
    public interface IMatcher<T> where T : ICommand
    {
        Task<bool> IsMatching(Update update);
    }
}
