using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebAPI.Commands
{
    public class StartCommand : CommandBase
    {
        private readonly IChatRepo _repo;

        public StartCommand(IChatRepo repo)
        {
            this._repo = repo;
        }
        public override string CommandName => "start";

        protected override bool CommandMatches(Update update)
        {
            if (UpdateIsCommand(update))
            {
                string messageText = update.Message.Text;

                if (FirstWordMatchesCommandName(messageText))
                {
                    return true;
                }
            }
            return false;
        }

        protected override async Task ExecuteCommandAsync(Update update)
        {
            var message = update.Message;

            string adminId = message.From.Id.ToString();

            string chatId = message.Chat.Id.ToString();

            await _repo.AddChat(chatId, adminId);
        }
    }
}
