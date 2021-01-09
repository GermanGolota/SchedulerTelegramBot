using Core;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AlertRepo : IAlertRepo
    {
        private readonly SchedulesContext _context;

        public AlertRepo(SchedulesContext context)
        {
            this._context = context;
        }
        public async Task UpdateJobId(Alert alert, string jobId)
        {
            _context.Attach(alert);

            alert.JobId = jobId;

            _context.SaveChanges();
        }
    }
}
