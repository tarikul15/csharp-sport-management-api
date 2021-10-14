using System.Threading.Tasks;
using SportsManagementAPi.Domain.Models;

namespace SportsManagementAPi.Domain.Services
{
    public interface IManagerService
    {
         Task<CreateManagerResponse> CreateUserAsync(Manager user);
         Task<Manager> FindByEmailAsync(string email);
    }
}