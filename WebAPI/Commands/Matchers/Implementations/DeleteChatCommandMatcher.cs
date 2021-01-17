using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Client;

namespace WebAPI.Commands.Verifiers
{
    public class DeleteChatCommandMatcher : AdminCommandMatcherBehaviour<DeleteChatCommand>
    {
        public DeleteChatCommandMatcher(IChatRepo repo, ITelegramClientAdapter client) :base( repo, client)
        {
            this.commandName = "deleteChat";
        }
    }
}
