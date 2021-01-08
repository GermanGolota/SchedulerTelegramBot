using Hangfire;
using Infrastructure.DTOs;
using Infrastructure.Repositories;
using SchedulerTelegramBot.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Hangfire;

namespace WebAPI.Jobs
{
    public class JobManager : IJobManager
    {
        private readonly IScheduleRepo _repo;
        private readonly ITelegramClientAdapter _client;

        public JobManager(IScheduleRepo repo, ITelegramClientAdapter client)
        {
            this._repo = repo;
            this._client = client;
        }

        public async Task SetupJobsForChat(ScheduleModel model, ChatId chat)
        {
            try
            {
                string chatId = chat.Identifier.ToString();
                await _repo.TryApplyScheduleToChat(model, chatId);
                int JobCount = 0;
                foreach (AlertModel alert in model.Alerts)
                {
                    RecurringJob.AddOrUpdate<HangfireActions>(GenerateJobId(chatId, JobCount),
                        x=>x.SendAlertMessage(alert.Message, chatId), alert.Cron);
                    JobCount++;
                }
            }
            catch (Exception ex)
            {
                await _client.SendTextMessageAsync(chat, $"Something went wrong, while adding jobs: {ex.Message}");
            }
        }
        private string GenerateJobId(string chatId, int count)
        {
            return $"{chatId}_{count}";
        }
    }
}
