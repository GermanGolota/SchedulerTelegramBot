using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Repositories
{
    public class ChatRepo : IChatRepo
    {
        private readonly SchedulesContext _context;

        public ChatRepo(SchedulesContext context)
        {
            this._context = context;
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
