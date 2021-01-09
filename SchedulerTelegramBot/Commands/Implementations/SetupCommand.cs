using Infrastructure.Repositories;
using SchedulerTelegramBot.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebAPI.Commands.Implementations
{
    public class SetupCommand : CommandBase
    {
        private readonly IChatRepo _repo;
        private readonly ITelegramClientAdapter _client;

        public SetupCommand(IChatRepo repo, ITelegramClientAdapter client)
        {
            this._repo = repo;
            this._client = client;
        }
        public override string CommandName => "setup";

        protected override bool CommandMatches(Update update)
        {
            if(UpdateIsCommand(update))
            {
                string message = update.Message.Text;
                if(FirstWordMatchesCommandName(message))
                {
                    return true;
                }
            }
            return false;
        }

        protected override async Task ExecuteCommandAsync(Update update)
        {
            var message = update.Message;

            var chatId = message.Chat.Id.ToString();

            string adminId = _repo.GetAdminIdOfChat(chatId);

            string userId = message.From.Id.ToString();

            if (userId.Equals(adminId))
            {
                var document = message.Document;
                if (document is not null)
                {
                    var docId = document.FileId;

                    //readFile

                    //addFileToDB

                }
                else
                {
                    await _client.SendTextMessageAsync(chatId, "Please attach a file");
                }
            }
            else
            {
                await _client.SendTextMessageAsync(chatId, "You don't have permission to do so");
            }
        }
    }
}
