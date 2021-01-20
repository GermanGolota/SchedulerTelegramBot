using Core;
using Core.Entities;
using Infrastructure.DTOs;
using Infrastructure.Exceptions;
using Infrastructure.Parsers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ScheduleRepo : IScheduleRepo
    {
        private readonly SchedulesContext _context;
        private readonly ICroneVerifier _croneVerifier;

        public ScheduleRepo(SchedulesContext context, ICroneVerifier croneVerifier)
        {
            this._context = context;
            this._croneVerifier = croneVerifier;
        }

        public async Task AddAlertsToSchedule(IEnumerable<Alert> alerts, int ScheduleId)
        {
            var schedule = _context.Schedules.Include(x => x.Alerts)
                .Where(x => x.ScheduleId == ScheduleId).FirstOrDefault();
            if(schedule is null)
            {
                throw new ScheduleDontExistException();
            }
            if (ConsistsOfProperCrons(alerts))
            {
                schedule.Alerts = schedule.Alerts.Union(alerts);
            }
            else
            {
                throw new CroneVerificationException();
            }

            _context.SaveChanges();
        }

     

        public async Task RemoveScheduleFromChat(string ChatId)
        {
            var chat = _context.Chats.Include(x => x.Schedule).FirstOrDefault(x => x.ChatId == ChatId);

            if (chat is null)
            {
                throw new ChatDontExistException();
            }

            var scheduleLink = chat.Schedule;

            if (scheduleLink is null)
            {
                throw new ScheduleDontExistException();
            }

            int scheduleLinkId = scheduleLink.ScheduleId;

            var schedule = _context.Schedules.Include(x => x.Alerts).FirstOrDefault(x => x.ScheduleId == scheduleLinkId);

            _context.Remove(schedule);

            _context.SaveChanges();
        }

        public async Task TryApplyScheduleToChat(Schedule schedule, string ChatId)
        {
            var chat = await _context.Chats.Include(x=>x.Schedule)
                .FirstAsync(chat => chat.ChatId == ChatId);
            if (chat is null)
            {
                throw new ChatDontExistException();
            }
            if (chat.Schedule is null)
            {
                var alerts = schedule.Alerts;
                if (ConsistsOfProperCrons(alerts))
                {
                    chat.Schedule = schedule;
                }
                else
                {
                    throw new CroneVerificationException();
                }
            }
            else
            {
                throw new ScheduleAlreadyAttachedException();
            }
            _context.SaveChanges();
        }

        public async Task UpdateScheduleName(string newName, int ScheduleId)
        {
            var schedule = _context.Schedules.Where(x => x.ScheduleId == ScheduleId).FirstOrDefault();

            if(schedule is null)
            {
                throw new ScheduleDontExistException();
            }

            schedule.Name = newName;

            _context.SaveChanges();
        }

        private bool ConsistsOfProperCrons(IEnumerable<Alert> alerts)
        {
            foreach (Alert alert in alerts)
            {
                bool validCron = _croneVerifier.VerifyCron(alert.Cron);
                bool notValidCron = !validCron;
                if (notValidCron)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
