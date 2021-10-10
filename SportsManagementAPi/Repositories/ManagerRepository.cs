using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportsManagementAPi.Domain.Models;
using SportsManagementAPi.Domain.Repositories;
using SportsManagementAPi.Repositories;

namespace SportsManagementAPi.Repositories
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly AppDbContext _context;

        public ManagerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Manager manager)
        {
            await _context.Managers.AddAsync(manager);
        }

        public async Task<Manager> FindByEmailAsync(string email)
        {
            return await _context.Managers.SingleOrDefaultAsync(u => u.Email == email);
        }
    }
}