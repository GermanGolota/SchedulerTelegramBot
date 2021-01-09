using Core;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            if(chatCount == 0)
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
                throw new Exception("This chat is alredy registered");
            }
        }

        public string GetAdminIdOfChat(string chatId)
        {
            var chat = _context.Chats.First(chat => chat.ChatId == chatId);

            if(chat is null)
            {
                throw new Exception("That chat is not yet in the system");
            }

            return chat.AdminId;
        }
    }
}
