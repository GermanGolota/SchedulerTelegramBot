using System.Collections.Generic;

namespace WebAPI.Commands
{
    public class MessageRepliesContainer
    {
        private readonly List<MessageReplyBase> messageReplies = new List<MessageReplyBase>();
        public MessageRepliesContainer(StartCommand start, SetupCommand setup, DeleteChatCommand delete)
        {
            messageReplies.Add(start);
            messageReplies.Add(setup);
            messageReplies.Add(delete);
        }
        public IReadOnlyList<MessageReplyBase> GetMessageReplies()
        {
            return messageReplies.AsReadOnly();
        }
    }
}
