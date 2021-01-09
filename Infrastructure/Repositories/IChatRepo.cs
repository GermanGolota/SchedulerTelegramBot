using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IChatRepo
    {
        string GetAdminIdOfChat(string ChatId);
        Task AddChat(string chatId, string adminId);
        List<Alert> GetAlertsOfChat(string chatId);
    }
}
