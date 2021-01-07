using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
