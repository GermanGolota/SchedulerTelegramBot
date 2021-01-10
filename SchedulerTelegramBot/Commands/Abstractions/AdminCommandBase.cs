using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Commands
{
    public abstract class AdminCommandBase : CommandBase
    {
        private readonly IChatRepo _repo;

        protected AdminCommandBase(IChatRepo repo)
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
