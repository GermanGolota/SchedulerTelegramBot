using Core.Entities;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IScheduleRepo
    {
        Task TryApplyScheduleToChat(Schedule schedule, string ChatId);
        Task RemoveScheduleFromChat(string ChatId);
    }
}
