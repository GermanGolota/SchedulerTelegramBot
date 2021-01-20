using System.IO;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebAPI.Client
{
    public interface ITelegramClientAdapter
    {
        Task BootUpClient();

        Task SendTextMessageAsync(ChatId chat, string message);

        Task<string> DownloadFileFromId(string fileId);

        Task SendStickerAsync(ChatId chat, string stickerLocation);
        Task SendTextFileAsync(ChatId chat, string fileContent);
    }
}