using Infrastructure.Repositories;
using System;
using Telegram.Bot.Types;

namespace WebAPI.Commands
{
    public abstract class CommandBase : MessageReplyBase
    {
        protected bool UpdateIsCommand(Update update)
        {
            if (UpdateContainsMessage(update))
            {
                var message = update.Message;
                var text = message.Text;
                if (text is not null)
                {
                    return StringIsCommand(text);
                }
                var caption = message.Caption;
                if(caption is not null)
                {
                    return StringIsCommand(caption);
                }
            }
            return false;
        }
        private bool UpdateContainsMessage(Update update)
        {
            return update.Message is not null;
        }
        private bool StringIsCommand(string str)
        {
           return str.StartsWith("/");
        }

        protected bool FirstWordMatchesCommandName(string str)
        {
            string message = str.Replace("/", "");

            string firstWord = GetFirstWord(message);

            return StringEqualsName(firstWord);
        }
        private string GetFirstWord(string str)
        {
            string output;
            if (SpaceExists(str))
            {
                int wordEndIndex = str.IndexOf(" ");
                output = str.Substring(wordEndIndex);
            }
            else
            {
                output = str;
            }
            return output;
        }
        private bool SpaceExists(string str)
        {
            return str.Contains(" ");
        }
        private bool StringEqualsName(string str)
        {
            int result = String.Compare(str, this.CommandName, ignoreCase: true);
            return result == 0;
        }
    }
}
