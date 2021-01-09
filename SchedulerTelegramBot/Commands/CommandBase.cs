﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebAPI.Commands
{
    public abstract class CommandBase : MessageReplyBase
    {
        public bool UpdateIsCommand(Update update)
        {
            try
            {
                bool isCommand = update.Message.Text.StartsWith("/");
                return isCommand;
            }
            catch
            {
                return false;
            }
        }
        public bool FirstWordMatchesCommandName(string str)
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
