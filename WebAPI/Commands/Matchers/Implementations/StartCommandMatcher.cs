using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebAPI.Commands.Verifiers
{
    public class StartCommandMatcher : StandardMatcherBehaviour<StartCommand>
    {
        public StartCommandMatcher()
        {
            this.commandName = "start";
        }
    }
}
