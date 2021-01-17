using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Commands.Verifiers;

namespace WebAPI.Commands
{
    public class CommandController<T> : ICommandController where T : ICommand
    {
        private readonly IMatcher<T> _matcher;
        private readonly T _coommand;
        public CommandController(IMatcher<T> matcher, T coommand)
        {
            this._matcher = matcher;
            this._coommand = coommand;
        }
        public async Task<CommandMatchResult> CheckCommand(Update update)
        {
            if (await _matcher.IsMatching(update))
            {
                await _coommand.Execute(update);
                return CommandMatchResult.Matching;
            }
            else
            {
                return CommandMatchResult.NotMatching;
            }
        }
    }
}
