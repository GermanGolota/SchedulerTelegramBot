using Microsoft.EntityFrameworkCore;

namespace Core.Entities
{
    public class Alert
    {
        public int AlertId { get; set; }
        public int ScheduleId { get; set; }
        public string Cron { get; set; }
        public string Message { get; set; }
        public string JobId { get; set; }
    }
}