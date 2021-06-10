using Infrastructure.Exceptions;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Client;
using WebAPI.Commands.Verifiers;

namespace WebAPI.Commands
{
    public class StartCommand : ICommand
    {
        private readonly IChatRepo _repo;
        private readonly ITelegramClient _client;
        private readonly ILogger<StartCommand> _logger;
        private const string StartupStickerId = @"CAACAgIAAxkBAAMrX_oDjl4RZ7SqvMaNBxaTese356AAAg0AA3EcFxMefvS-UNPkwR4E";
        public StartCommand(IChatRepo repo, ITelegramClient client, ILogger<StartCommand> logger)
        {
            this._repo = repo;
            this._client = client;
            this._logger = logger;
        }

        public async Task Execute(Update update)
        {
            var message = update.Message;

            string adminId = message.From.Id.ToString();

            string chatId = message.Chat.Id.ToString();

            try
            {
                await _repo.AddChat(chatId, adminId);

                await _client.SendStickerAsync(chatId, StartupStickerId);

                await _client.SendTextMessageAsync(chatId, StandardMessages.ChatRegistration);
            }
            catch (DataAccessException exc)
            {
                await _client.SendTextMessageAsync(chatId, exc.Message);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "Failed to add chat");
                throw;
            }
        }
    }
}
