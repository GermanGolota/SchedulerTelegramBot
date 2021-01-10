using Core.Entities;
using Hangfire;
using Infrastructure.DTOs;
using Infrastructure.Repositories;
using SchedulerTelegramBot.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Hangfire;

namespace WebAPI.Jobs
{
    public class JobManager : IJobManager
    {
        private readonly IScheduleRepo _scheduleRepo;
        private readonly IChatRepo _chatRepo;
        private readonly IAlertRepo _alertRepo;

        public JobManager(IScheduleRepo schedule, IChatRepo chat, IAlertRepo alert)
        {
            this._scheduleRepo = schedule;
            this._chatRepo = chat;
            this._alertRepo = alert;
        }

        public async Task DeleteJobsFromChat(ChatId chatId)
        {
            List<Alert> alerts = _chatRepo.GetAlertsOfChat(chatId);

            foreach (Alert alert in alerts)
            {
                string jobId = alert.JobId;
                RecurringJob.RemoveIfExists(jobId);
            }

            try
            {
                await _scheduleRepo.RemoveScheduleFromChat(chatId);
            }
            catch
            {
                throw;
            }
        }

        public async Task SetupJobsForChat(ScheduleModel model, ChatId chat)
        {
            try
            {
                string chatId = chat.Identifier.ToString();
                await _scheduleRepo.TryApplyScheduleToChat(model, chatId);

                List<Alert> alerts = _chatRepo.GetAlertsOfChat(chatId);

                int JobCount = 0;
                foreach (Alert alert in alerts)
                {
                    string jobId = GenerateJobId(chatId, JobCount);
                    RecurringJob.AddOrUpdate<HangfireActions>(jobId, x=>x.SendAlertMessage(alert.Message, chatId), alert.Cron);
                    await _alertRepo.UpdateJobId(alert.AlertId, jobId);
                    JobCount++;
                }
            }
            catch
            {
                throw;
            }
        }
        private string GenerateJobId(string chatId, int count)
        {
            return $"{chatId}_{count}";
        }
    }
}
