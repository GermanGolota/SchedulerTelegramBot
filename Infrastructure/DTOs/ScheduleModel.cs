using System.Collections.Generic;

namespace Infrastructure.DTOs
{
    public class ScheduleModel
    {
        public List<AlertModel> Alerts { get; set; }
        public string Name { get; set; }
    }
}
