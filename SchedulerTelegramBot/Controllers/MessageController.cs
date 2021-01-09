using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Commands;

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
