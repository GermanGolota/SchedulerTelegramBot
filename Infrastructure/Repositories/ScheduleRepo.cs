using Core;
using Infrastructure.DTOs;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ScheduleRepo : IScheduleRepo
    {
        private readonly SchedulesContext _context;
        private readonly IModelConverter _converter;

        public ScheduleRepo(SchedulesContext context, IModelConverter converter)
        {
            this._context = context;
            this._converter = converter;
        }
        public async Task TryApplyScheduleToChat(ScheduleModel schedule, string ChatId)
        {
            var chat = await _context.Chats.FirstAsync(chat => chat.ChatId == ChatId);
            if(chat is null)
            {
                throw new ChatDontExistException();
            }
            if(chat.Schedule is null)
            {
                chat.Schedule = _converter.ConvertScheduleFromDTO(schedule);
            }
            else
            {
                throw new ScheduleAlreadyAttachedException();
            }
            _context.SaveChanges();
        }
    }
}
