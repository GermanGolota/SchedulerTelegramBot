using System.IO;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebAPI.Client
{
    public interface ITelegramClient
    {
        Task BootUpClient();

        Task SendTextMessageAsync(ChatId chat, string message);

        Task<string> DownloadFileFromId(string fileId);

        Task SendStickerAsync(ChatId chat, string stickerLocation);
        Task SendTextFileAsync(ChatId chat, string fileContent, string fileName);
        Task SetupWebhook(string webhookUrl);
    }
}