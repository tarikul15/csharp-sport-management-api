using System.Threading.Tasks;
using SportsManagementAPi.Domain.Models;

namespace SportsManagementAPi.Domain.Services
{
    public interface IUserService
    {
         Task<CreateUserResponse> CreateUserAsync(User user, params ApplicationRole[] userRoles);
         Task<User> FindByEmailAsync(string email);
    }
}