using System.Collections.Generic;

namespace WebAPI.Commands
{
    public class MessageRepliesContainer
    {
        private readonly List<MessageReplyBase> messageReplies = new List<MessageReplyBase>();
        public MessageRepliesContainer(StartCommand start, SetupCommand setup, DeleteChatCommand delete,
            DeleteScheduleCommand schDelete)
        {
            messageReplies.Add(start);
            messageReplies.Add(setup);
            messageReplies.Add(delete);
            messageReplies.Add(schDelete);
        }
        public IReadOnlyList<MessageReplyBase> GetMessageReplies()
        {
            return messageReplies.AsReadOnly();
        }
    }
}
