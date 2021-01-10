using Core;
using Core.Entities;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ChatRepo : IChatRepo
    {
        private readonly SchedulesContext _context;

        public ChatRepo(SchedulesContext context)
        {
            this._context = context;
        }

        public async Task AddChat(string chatId, string adminId)
        {
            int chatCount = _context.Chats.Where(x => x.ChatId == chatId).Count();

            if (chatCount == 0)
            {
                Chat chat = new Chat
                {
                    AdminId = adminId,
                    ChatId = chatId
                };
                _context.Chats.Add(chat);

                _context.SaveChanges();
            }
            else
            {
                throw new ChatAlreadyExistsException();
            }
        }

        public async Task DeleteChat(string chatId)
        {
            var chat = _context.Chats.Include(x => x.Schedule).Where(x => x.ChatId == chatId).FirstOrDefault();

            ValidateChatExistance(chat);

            _context.Remove(chat);

            _context.SaveChanges();
        }

        private void ValidateChatExistance(Chat chat)
        {
            if (chat is null)
            {
                throw new ChatDontExistException();
            }
        }

        public string GetAdminIdOfChat(string chatId)
        {
            var chat = _context.Chats.Where(chat => chat.ChatId == chatId).Select(
                x =>new
                {
                    adminId = x.AdminId
                }
            ).FirstOrDefault();

            if (chat is null)
            {
                throw new ChatDontExistException();
            }

            return chat.adminId;
        }
        public List<Alert> GetAlertsOfChat(string chatId)
        {
            var chat = _context.Chats.Include(chat => chat.Schedule).First(chat => chat.ChatId == chatId);

            if (chat is null)
            {
                throw new ChatDontExistException();
            }

            var alerts = _context.Schedules.Include(sch => sch.Alerts).Where(sch => sch.ScheduleId == chat.ScheduleId)
                .Select(sch => sch.Alerts).First();

            List<Alert> output = alerts.ToList();

            return output;
        }
    }
}
