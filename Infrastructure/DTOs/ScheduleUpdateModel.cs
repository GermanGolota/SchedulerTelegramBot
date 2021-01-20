using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DTOs
{
    public class ScheduleUpdateModel
    {
        public string NewName { get; set; }
        public List<AlertModel> Alerts { get; set; }
    }
}
