using Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Client;

namespace WebAPI.Commands
{
    public class UpdateManager : IUpdateManager
    {
        private readonly ITelegramClientAdapter _client;

        public UpdateManager(ITelegramClientAdapter client)
        {
            this._client = client;
        }
        public async Task<string> GetFileContentsFrom(Update update)
        {
            var message = update.Message;

            var document = message.Document;
            if (document is null)
            {
                throw new NoFileAttachedException();
            }

            var docId = document.FileId;

            string fileContent = await GetFileContent(docId);
            return fileContent;
        }
        private async Task<string> GetFileContent(string fileId)
        {
            string fileLocation = await _client.DownloadFileFromId(fileId);
            using (FileStream stream = new FileStream(fileLocation, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    string fileContent = sr.ReadToEnd();
                    return fileContent;
                }
            }
        }
    }
}
