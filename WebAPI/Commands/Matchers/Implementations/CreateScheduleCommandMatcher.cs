using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Commands.Verifiers;

namespace WebAPI.Commands
{
    public class CreateScheduleCommandMatcher : RequestMatcherBase<CreateScheduleCommand>
    {
        private string commandName = "createSchedule";
        public override async Task<bool> IsMatching(Update update)
        {
            if (UpdateIsCommand(update))
            {
                string messageText = update.Message.Text ?? "";

                if (FirstWordMatchesCommandName(messageText, commandName))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
