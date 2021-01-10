using Infrastructure.Repositories;
using SchedulerTelegramBot.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebAPI.Commands
{
    public class DeleteChatCommand : CommandBase
    {
        private readonly ITelegramClientAdapter _client;
        private readonly IChatRepo _repo;
        public string ChatIdToBeDeleted { get; private set; }

        public DeleteChatCommand(ITelegramClientAdapter client, IChatRepo repo)
        {
            this._client = client;
            this._repo = repo;
        }
        public override string CommandName => "deleteChat";

        protected override async Task<bool> CommandMatches(Update update)
        {
            if (UpdateIsCommand(update))
            {
                var message = update.Message;
                string messageText = message.Text;
                if (FirstWordMatchesCommandName(messageText))
                {
                    ChatIdToBeDeleted = message.Chat.Id.ToString();
                    string userId = message.From.Id.ToString();
                    if (!UserIsAdminInChat(userId, ChatIdToBeDeleted))
                    {
                        await _client.SendTextMessageAsync(ChatIdToBeDeleted, "You don't have permission to do that");
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        protected override async Task ExecuteCommandAsync(Update update)
        {
            await _repo.DeleteChat(ChatIdToBeDeleted);
            await _client.SendTextMessageAsync(ChatIdToBeDeleted, "Successfully deleted chat");
        }
        private bool UserIsAdminInChat(string userId, string chatId)
        {
            string adminId = _repo.GetAdminIdOfChat(chatId);
            return userId.Equals(adminId);
        }
    }
}
