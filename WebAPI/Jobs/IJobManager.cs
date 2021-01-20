using Infrastructure.DTOs;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebAPI.Jobs
{
    public interface IJobManager
    {
        Task SetupJobsForChat(ScheduleModel model, ChatId chat);
        Task DeleteJobsFromChat(ChatId chat);
        Task AddJobsToExistingChat(ChatId chat, ScheduleUpdateModel model);
    }
}
