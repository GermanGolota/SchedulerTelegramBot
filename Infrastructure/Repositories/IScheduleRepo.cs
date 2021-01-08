using Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IScheduleRepo
    {
        Task TryApplyScheduleToChat(ScheduleModel schedule, string ChatId); 
    }
}
