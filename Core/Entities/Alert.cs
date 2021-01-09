using Microsoft.EntityFrameworkCore;

namespace Core.Entities
{
    [Owned]
    public class Alert
    {
        public int ScheduleId { get; set; }
        public string Cron { get; set; }
        public string Message { get; set; }
        public string JobId { get; set; }
    }
}