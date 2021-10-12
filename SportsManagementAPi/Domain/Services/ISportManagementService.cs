using System;
using System.Collections.Generic;
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

        Task<Player> FindPlayerByIdAsync(Guid id);

        Task<CreateScheduleResponse> CreateScheduleAsync(Schedule schedule);

        Task<CreateResultResponse> CreateResultAsync(Result result);

        Task<GetPlayersResponse> GetPlayersByManagerId(Guid managerId);

        Task<DeletePlayerResponse> DeletePlayerById(Guid playerId, Guid managerId);

        Task<PatchPlayerResponse> PatchPlayer(Player player);
    }
}
