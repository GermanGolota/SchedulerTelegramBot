using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Commands.Verifiers
{
    public abstract class AdminCommandMatcherBase<T> : RequestMatcherBase<T> where T : MessageReplyBase
    {
        private readonly IChatRepo _repo;

        public AdminCommandMatcherBase(MessageReplyBase command, IChatRepo repo):base(command)
        {
            this._repo = repo;
        }
        protected bool UserIsAdminInChat(string userId, string chatId)
        {
            string adminId = _repo.GetAdminIdOfChat(chatId);
            return userId.Equals(adminId);
        }
    }
}
