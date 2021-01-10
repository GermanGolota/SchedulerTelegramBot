using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IChatRepo
    {
        string GetAdminIdOfChat(string ChatId);
        Task AddChat(string chatId, string adminId);
        Task DeleteChat(string chatId);
        List<Alert> GetAlertsOfChat(string chatId);
    }
}
