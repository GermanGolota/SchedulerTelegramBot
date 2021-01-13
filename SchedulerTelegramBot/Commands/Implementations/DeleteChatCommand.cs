﻿using Infrastructure.Repositories;
using SchedulerTelegramBot.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;
using WebAPI.Commands.Verifiers;

namespace WebAPI.Commands
{
    public class DeleteChatCommand : MessageReplyBase
    {
        private readonly IMatcher<DeleteChatCommand> _matcher;
        private readonly ITelegramClientAdapter _client;
        private readonly IChatRepo _repo;
        private readonly ILogger<DeleteChatCommand> _logger;

        private string chatIdToBeDeleted;

        public DeleteChatCommand(IMatcher<DeleteChatCommand> matcher, ITelegramClientAdapter client,
            IChatRepo repo, ILogger<DeleteChatCommand> logger)
        {
            this._matcher = matcher;
            this._client = client;
            this._repo = repo;
            this._logger = logger;
        }
        public override string CommandName => "deleteChat";

        protected override async Task<bool> CommandMatches(Update update)
        {
            return await _matcher.IsMatching(update);
        }

        protected override async Task ExecuteCommandAsync(Update update)
        {
            try
            {
                await _repo.DeleteChat(chatIdToBeDeleted);

                await _client.SendTextMessageAsync(chatIdToBeDeleted, StandardMessages.ChatDeletionSuccess);
            }
            catch(DataAccessException exc)
            {
                await _client.SendTextMessageAsync(chatIdToBeDeleted, exc.Message);
            }
            catch(Exception exc)
            {
                _logger.LogError(exc, "Were not able to delete chat");
            }
        }
    }
}
