using System.Collections.Generic;

namespace WebAPI.Commands
{
    public class CommandsContainer
    {
        private readonly List<CommandBase> messageReplies = new List<CommandBase>();
        public CommandsContainer(StartCommand start, SetupCommand setup, DeleteChatCommand delete,
            DeleteScheduleCommand schDelete)
        {
            messageReplies.Add(start);
            messageReplies.Add(setup);
            messageReplies.Add(delete);
            messageReplies.Add(schDelete);
        }
        public IReadOnlyList<CommandBase> GetMessageReplies()
        {
            return messageReplies.AsReadOnly();
        }
    }
}
