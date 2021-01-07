using Microsoft.AspNetCore.Mvc;
using SchedulerTelegramBot.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace SchedulerTelegramBot.Controllers
{
    [ApiController]
    public class MessageController:ControllerBase
    {
        private readonly ITelegramClientAdapter _client;
        public MessageController(ITelegramClientAdapter client)
        {
            this._client = client;
        }
        [HttpPost("api/message/update")]
        public async Task<IActionResult> Update([FromBody]Update update)
        {
            if(update.Message!=null)
            {
                var chat = update.Message.Chat.Id;

                await _client.SendTextMessageAsync(chat, "Message received");

            }
            return Ok();
        }
    }
}
