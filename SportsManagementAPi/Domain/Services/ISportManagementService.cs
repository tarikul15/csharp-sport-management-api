using System.Threading.Tasks;
using SportsManagementAPi.Domain.Models;

namespace SportsManagementAPi.Domain.Services
{
    public interface ISportManagementService
    {
        Task<CreateTeamResponse> CreateTeamAsync(Team user);
        //Task<Manager> FindByEmailAsync(string email);
    }
}
