using System.Threading.Tasks;
using SportsManagementAPi.Domain.Models;

namespace SportsManagementAPi.Domain.Services
{
    public interface ISportManagementService
    {
        Task<CreateTeamResponse> CreateTeamAsync(Team team);
        
        Task<Team> FindTeamByNameAsync(string name);

        Task<CreatePlayerResponse> CreatePlayerAsync(Player player);

        Task<Player> FindPlayerByNameAsync(string name);
    }
}
