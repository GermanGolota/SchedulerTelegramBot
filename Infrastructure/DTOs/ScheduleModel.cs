using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DTOs
{
    public class ScheduleModel
    {
        public List<AlertModel> Alerts { get; set; }
        public string Name { get; set; }
    }
}
