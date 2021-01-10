using Core.Entities;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IAlertRepo
    {
        Task UpdateJobId(int alertId, string jobId); 
    }
}
