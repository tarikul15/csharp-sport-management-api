using System.Threading.Tasks;
using SportsManagementAPi.Domain.Models;
using SportsManagementAPi.Domain.Repositories;
using SportsManagementAPi.Domain.Services;
using SportsManagementAPi.Domain.Security;

namespace SportsManagementAPi.Services
{
    public class SportManagementService : ISportManagementService
    {
        private readonly ISportManagementRepository _sportManagementRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SportManagementService(ISportManagementRepository sportManagementRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _sportManagementRepository = sportManagementRepository;
        }

        public async Task<CreateTeamResponse> CreateTeamAsync(Team team)
        {
            var existingTeam = await _sportManagementRepository.FindTeamByName(team.Name);
            if (existingTeam != null)
            {
                return new CreateTeamResponse(false, "Name already in use.", null);
            }


            await _sportManagementRepository.AddTeamAsync(team);
            await _unitOfWork.CompleteAsync();

            return new CreateTeamResponse(true, null, team);
        }

        public async Task<Team> FindTeamByNameAsync(string name)
        {
            return await _sportManagementRepository.FindTeamByName(name);
        }

        public async Task<CreatePlayerResponse> CreatePlayerAsync(Player player)
        {
            var existingPlayer = await _sportManagementRepository.FindPlayerByName(player.Name);
            if (existingPlayer != null)
            {
                return new CreatePlayerResponse(false, "Name already in use.", null);
            }

            await _sportManagementRepository.AddPlayerAsync(player);
            await _unitOfWork.CompleteAsync();

            return new CreatePlayerResponse(true, null, player);
        }

        public async Task<Player> FindPlayerByNameAsync(string name)
        {
            return await _sportManagementRepository.FindPlayerByName(name);
        }
    }
}