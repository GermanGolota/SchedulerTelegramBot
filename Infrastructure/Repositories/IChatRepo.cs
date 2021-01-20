using Core.Entities;
using Infrastructure.DTOs;
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
        Task<int> GetScheduleIdOfChat(string ChatId);
        Task<ScheduleModel> GetScheduleForChat(string chatId);
    }
}
