using Core.Entities;
using Hangfire;
using Infrastructure.DTOs;
using Infrastructure.Exceptions;
using Infrastructure.Repositories;
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
        private readonly IModelConverter _converter;

        public JobManager(IScheduleRepo schedule, IChatRepo chat,
            IModelConverter converter)
        {
            this._scheduleRepo = schedule;
            this._chatRepo = chat;
            this._converter = converter;
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
            string chatId = chat.Identifier.ToString();

            var dtoAlerts = model.Alerts;

            List<string> jobIds = SetupJobs(dtoAlerts, chatId);

            try
            {
                Schedule schedule = ConvertDto(model);
                var alerts = schedule.Alerts;

                SetJobIdsToAlerts(jobIds, alerts);

                await _scheduleRepo.TryApplyScheduleToChat(schedule, chatId);
            }
            catch
            {
                DeleteCreatedJobs(jobIds);
                throw;
            }
        }
        //returns list of ids of created jobs
        private List<string> SetupJobs(List<AlertModel> dtoAlerts, string chatId, int initialCount = 0)
        {
            List<string> jobIds = new List<string>();
            int JobCount = initialCount;
            for (int i = 0; i < dtoAlerts.Count; i++)
            {
                var alert = dtoAlerts[i];
                string jobId = GenerateJobId(chatId, JobCount);
                RecurringJob.AddOrUpdate<HangfireActions>
                    (jobId, x => x.SendAlertMessage(alert.Message, chatId), alert.Cron);
                JobCount++;
                jobIds.Add(jobId);
            }
            return jobIds;
        }
        private string GenerateJobId(string chatId, int count)
        {
            return $"{chatId}_{count}";
        }
        private Schedule ConvertDto(ScheduleModel dto)
        {
            return _converter.ConvertScheduleFromDTO(dto);
        }
        private void SetJobIdsToAlerts(List<string> jobIds, IEnumerable<Alert> alerts)
        {
            int i = 0;
            foreach (Alert alert in alerts)
            {
                alert.JobId = jobIds[i];
                i++;
            }
        }
        private void DeleteCreatedJobs(List<string> jobIds)
        {
            foreach (string jobId in jobIds)
            {
                RecurringJob.RemoveIfExists(jobId);
            }
        }

        public async Task AddJobsToChatWithExistingSchedule(ChatId chat, ScheduleUpdateModel model)
        {
            string chatId = chat.Identifier.ToString();

            var dtoAlerts = model.Alerts;

            string newName = model.NewName;

            int scheduleId = await _chatRepo.GetScheduleIdOfChat(chatId);

            int initialCount = await _scheduleRepo.GetAlertsCountOf(scheduleId);

            List<string> jobIds = SetupJobs(dtoAlerts, chatId, initialCount);

            try
            {
                var alerts = ConvertAlerts(dtoAlerts);

                SetJobIdsToAlerts(jobIds, alerts);

                await ApplyNewName(newName, scheduleId);

                await _scheduleRepo.AddAlertsToSchedule(alerts, scheduleId);
            }
            catch
            {
                DeleteCreatedJobs(jobIds);
                throw;
            }
        }
        private List<Alert> ConvertAlerts(List<AlertModel> alerts)
        {
            List<Alert> output = new List<Alert>();
            foreach (AlertModel alert in alerts)
            {
                output.Add(_converter.ConvertAlertFromDto(alert));
            }
            return output;
        }
        private async Task ApplyNewName(string newName, int scheduleId)
        {
            if (newName is not null)
            {
                await _scheduleRepo.UpdateScheduleName(newName, scheduleId);
            }
        }
    }
}
