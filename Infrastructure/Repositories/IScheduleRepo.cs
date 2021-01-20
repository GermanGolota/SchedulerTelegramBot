using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IScheduleRepo
    {
        Task TryApplyScheduleToChat(Schedule schedule, string ChatId);
        Task RemoveScheduleFromChat(string ChatId);
        Task AddAlertsToSchedule(IEnumerable<Alert> alerts, int ScheduleId);
        Task UpdateScheduleName(string newName, int ScheduleId);
    }
}
