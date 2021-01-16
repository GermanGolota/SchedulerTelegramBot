using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Commands.Verifiers
{
    public abstract class AdminCommandMatcherBase<T> : RequestMatcherBase<T> where T : ICommand
    {
        private readonly IChatRepo _repo;

        public AdminCommandMatcherBase(IChatRepo repo)
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
