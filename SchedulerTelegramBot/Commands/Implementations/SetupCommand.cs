using Infrastructure.Repositories;
using SchedulerTelegramBot.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Newtonsoft.Json;
using Infrastructure.DTOs;
using WebAPI.Jobs;

namespace WebAPI.Commands
{
    public class SetupCommand : CommandBase
    {
        private readonly IChatRepo _repo;
        private readonly ITelegramClientAdapter _client;
        private readonly IJobManager _jobs;

        public SetupCommand(IChatRepo repo, ITelegramClientAdapter client, IJobManager jobs)
        {
            this._repo = repo;
            this._client = client;
            this._jobs = jobs;
        }
        public override string CommandName => "setup";

        protected override bool CommandMatches(Update update)
        {
            if(UpdateIsCommand(update))
            {
                string message = update.Message.Text;
                if(FirstWordMatchesCommandName(message))
                {
                    return true;
                }
            }
            return false;
        }

        protected override async Task ExecuteCommandAsync(Update update)
        {
            var message = update.Message;

            var chatId = message.Chat.Id.ToString();

            string adminId = _repo.GetAdminIdOfChat(chatId);

            string userId = message.From.Id.ToString();

            if (userId.Equals(adminId))
            {
                var document = message.Document;
                if (document is not null)
                {
                    var docId = document.FileId;


                    string fileContent;
                    using (FileStream file = await _client.GetFileStreamFromId(docId))
                    {
                        using (StreamReader sr = new StreamReader(file))
                        {
                            fileContent = sr.ReadToEnd();
                        }
                    }
                    try
                    {
                        var model = JsonConvert.DeserializeObject<ScheduleModel>(fileContent);
                        try
                        {
                            await _jobs.SetupJobsForChat(model, chatId);

                            await _client.SendTextMessageAsync(chatId, "Schedule have been succesfully set");
                        }
                        catch(Exception exc)
                        {
                            await _client.SendTextMessageAsync(chatId, exc.Message);
                        }
                    }
                    catch
                    {
                        await _client.SendTextMessageAsync(chatId, "Data in the file is not valid");
                    }
                }
                else
                {
                    await _client.SendTextMessageAsync(chatId, "Please attach a file");
                }
            }
            else
            {
                await _client.SendTextMessageAsync(chatId, "You don't have permission to do so");
            }
        }
    }
}
