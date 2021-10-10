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
            var existingUser = await _sportManagementRepository.FindByName(team.Name);
            if (existingUser != null)
            {
                return new CreateTeamResponse(false, "Name already in use.", null);
            }


            await _sportManagementRepository.AddTeamAsync(team);
            await _unitOfWork.CompleteAsync();

            return new CreateTeamResponse(true, null, team);
        }

    }
}