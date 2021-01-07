using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Entities
{
    public class Chat
    {
        [Key]
        public string ChatId { get; set; }
        public string AdminId { get; set; }
        public int? ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
    }
}
