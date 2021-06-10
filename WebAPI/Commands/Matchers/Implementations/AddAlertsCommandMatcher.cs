using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Client;

namespace WebAPI.Commands.Verifiers
{
    public class AddAlertsCommandMatcher:FileAdminCommandMatcherBehaviour<AddAlertCommand>
    {
        public AddAlertsCommandMatcher(ITelegramClient client, IChatRepo repo)
            :base(client, repo, CommandNames.AddAlerts)
        {

        }
    }
}
