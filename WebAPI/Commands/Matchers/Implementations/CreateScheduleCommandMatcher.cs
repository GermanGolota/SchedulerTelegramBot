using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Commands.Verifiers;

namespace WebAPI.Commands
{
    public class CreateScheduleCommandMatcher : StandardMatcherBehaviour<CreateScheduleCommand>
    {
        public CreateScheduleCommandMatcher()
        {
            this.commandName = "createSchedule";
        }
    }
}
