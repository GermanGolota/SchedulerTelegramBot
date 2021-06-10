using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core
{
    public class SchedulesContext : DbContext
    {
        public SchedulesContext(DbContextOptions<SchedulesContext> options) : base(options)
        {

        }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Alert> Alerts { get; set; }
    }
}
