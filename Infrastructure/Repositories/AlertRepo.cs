using Core;
using Core.Entities;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AlertRepo : IAlertRepo
    {
        private readonly SchedulesContext _context;

        public AlertRepo(SchedulesContext context)
        {
            this._context = context;
        }
        public async Task UpdateJobId(int alertId, string jobId)
        {
            var alert = _context.Find<Alert>(alertId);

            alert.JobId = jobId;

            _context.SaveChanges();
        }
    }
}
