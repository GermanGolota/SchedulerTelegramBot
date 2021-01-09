using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Commands
{
    public class MessageRepliesContainer
    {
        private readonly List<MessageReplyBase> messageReplies = new List<MessageReplyBase>();
        public MessageRepliesContainer(StartCommand start)
        {
            messageReplies.Add(start);
        }
        public IReadOnlyList<MessageReplyBase> GetMessageReplies()
        {
            return messageReplies.AsReadOnly();
        }
    }
}
