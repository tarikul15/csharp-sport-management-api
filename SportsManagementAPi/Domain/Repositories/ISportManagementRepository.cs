using System.Threading.Tasks;
using SportsManagementAPi.Domain.Models;

namespace SportsManagementAPi.Domain.Repositories
{
    public interface ISportManagementRepository
    {
        Task AddTeamAsync(Team team);
        
        Task<Team> FindTeamByName(string name);

        Task AddPlayerAsync(Player player);

        Task<Player> FindPlayerByName(string name);
    }
}