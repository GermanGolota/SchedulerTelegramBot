using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using SchedulerTelegramBot.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Commands;
using WebAPI.Jobs;

namespace SchedulerTelegramBot.Controllers
{
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MessageRepliesContainer _container;

        public MessageController(MessageRepliesContainer container)
        {
            this._container = container;
        }
        [HttpPost("api/message/update")]
        public async Task<IActionResult> Update([FromBody] Update update)
        {

            var replies = _container.GetMessageReplies();
            foreach (var reply in replies)
            {
                CommandMatchResult result = await reply.ExecuteCommandIfMatched(update);
                if (result.Equals(CommandMatchResult.Matching))
                {
                    break;
                }
            }

            return Ok();
        }

    }
}
