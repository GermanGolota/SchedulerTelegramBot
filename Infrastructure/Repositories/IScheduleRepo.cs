using Infrastructure.DTOs;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IScheduleRepo
    {
        Task TryApplyScheduleToChat(ScheduleModel schedule, string ChatId); 
    }
}
