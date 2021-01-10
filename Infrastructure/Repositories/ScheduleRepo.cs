using Core;
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
        private readonly IModelConverter _converter;
        private readonly ICroneVerifier _croneVerifier;

        public ScheduleRepo(SchedulesContext context, IModelConverter converter, ICroneVerifier croneVerifier)
        {
            this._context = context;
            this._converter = converter;
            this._croneVerifier = croneVerifier;
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

        public async Task TryApplyScheduleToChat(ScheduleModel scheduleDTO, string ChatId)
        {
            var chat = await _context.Chats.FirstAsync(chat => chat.ChatId == ChatId);
            if (chat is null)
            {
                throw new ChatDontExistException();
            }
            if (chat.Schedule is null)
            {
                var alerts = scheduleDTO.Alerts;
                if (ConsistsOfProperCrons(alerts))
                {
                    var schedule = _converter.ConvertScheduleFromDTO(scheduleDTO);
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
        private bool ConsistsOfProperCrons(List<AlertModel> alerts)
        {
            foreach (AlertModel alert in alerts)
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
