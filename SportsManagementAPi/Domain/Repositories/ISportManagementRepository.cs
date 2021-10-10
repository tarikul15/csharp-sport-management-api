using System.Threading.Tasks;
using SportsManagementAPi.Domain.Models;

namespace SportsManagementAPi.Domain.Repositories
{
    public interface ISportManagementRepository
    {
        Task AddTeamAsync(Team team);
        
        Task<Team> FindByName(string name);
    }
}