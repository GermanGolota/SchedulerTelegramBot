﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebAPI.Commands.Verifiers
{
    public class StandardMatcherBehaviour<T> : RequestMatcherBase<T> where T : ICommand
    {
        private string commandName { get; init; }
        public StandardMatcherBehaviour(string commandName)
        {
            this.commandName = commandName;
        }
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
