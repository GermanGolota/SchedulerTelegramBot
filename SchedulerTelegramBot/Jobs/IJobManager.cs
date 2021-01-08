using Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebAPI.Jobs
{
    public interface IJobManager
    {
        Task SetupJobsForChat(ScheduleModel model, ChatId chat);
    }
}
