using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportsManagementAPi.Domain.Models;
using SportsManagementAPi.Domain.Repositories;
using SportsManagementAPi.Repositories;

namespace SportsManagementAPi.Repositories
{
    public class SportManagementRepository : ISportManagementRepository
    {
        private readonly AppDbContext _context;

        public SportManagementRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddTeamAsync(Team team)
        {
            await _context.Teams.AddAsync(team);
        }

        public async Task<Team> FindByName(string name)
        {
            return await _context.Teams.SingleOrDefaultAsync(u => u.Name == name);

        }

    }
}