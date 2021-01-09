using System.Collections.Generic;

namespace WebAPI.Commands
{
    public class MessageRepliesContainer
    {
        private readonly List<MessageReplyBase> messageReplies = new List<MessageReplyBase>();
        public MessageRepliesContainer(StartCommand start, SetupCommand setup)
        {
            messageReplies.Add(start);
            messageReplies.Add(setup);
        }
        public IReadOnlyList<MessageReplyBase> GetMessageReplies()
        {
            return messageReplies.AsReadOnly();
        }
    }
}
