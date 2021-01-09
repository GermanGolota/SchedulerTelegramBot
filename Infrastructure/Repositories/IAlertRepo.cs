using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IAlertRepo
    {
        Task UpdateJobId(Alert alert, string jobId); 
    }
}
