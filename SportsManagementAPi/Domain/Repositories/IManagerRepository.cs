using System.Threading.Tasks;
using SportsManagementAPi.Domain.Models;

namespace SportsManagementAPi.Domain.Repositories
{
    public interface IManagerRepository
    {
        Task AddAsync(Manager user);
        Task<Manager> FindByEmailAsync(string email);
    }
}