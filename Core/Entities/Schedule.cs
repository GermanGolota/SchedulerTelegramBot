using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Alert> Alerts { get; set; }
    }
}
