using Core;
using Infrastructure.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                throw new Exception("No such chat");
            }
            if(chat.Schedule is null)
            {
                chat.Schedule = _converter.ConvertScheduleFromDTO(schedule);
            }
            else
            {
                throw new Exception("This chat already has schedule attached to it");
            }
            _context.SaveChanges();
        }
    }
}
