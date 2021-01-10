using Infrastructure.Exceptions;
using Infrastructure.Repositories;
using SchedulerTelegramBot.Client;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebAPI.Commands
{
    public class StartCommand : CommandBase
    {
        private readonly IChatRepo _repo;
        private readonly ITelegramClientAdapter _client;
        private const string StartupStickerId = @"CAACAgIAAxkBAAMrX_oDjl4RZ7SqvMaNBxaTese356AAAg0AA3EcFxMefvS-UNPkwR4E";
        public StartCommand(IChatRepo repo, ITelegramClientAdapter client)
        {
            this._repo = repo;
            this._client = client;
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

            try
            {
                await _repo.AddChat(chatId, adminId);

                await _client.SendStickerAsync(chatId, StartupStickerId);

                await _client.SendTextMessageAsync(chatId, "Activated");
            }
            catch(ChatAlreadyExistsException)
            {
                await _client.SendTextMessageAsync(chatId, "Already activated, sorry");
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
