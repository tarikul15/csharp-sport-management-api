using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SportsManagementAPi.Domain.Models;

namespace SportsManagementAPi.Domain.Repositories
{
    public interface ISportManagementRepository
    {
        Task AddTeamAsync(Team team);
        
        Task<Team> FindTeamByName(string name);

        Task<Team> FindTeamById(Guid teamId);

        Task AddPlayerAsync(Player player);

        Task<Player> FindPlayerByName(string name);

        Task<Player> FindPlayerById(Guid playerId);

        Task AddScheduleAsync(Schedule schedule);

        Task<Schedule> FindScheduleByTimeAndTeams(DateTime time, Guid homeTeamId, Guid awayTeamId);

        Task<Schedule> FindScheduleByGameId(Guid gameId);
        
        Task AddResultAsync(Result Result);

        Task<List<Player>> GetPlayersByManagerId(Guid managerId);

        Task DeletePlayerById(Guid id);

        Task PatchPlayer(Player player);

        Task<List<Schedule>> GetScheduleWithResultsByManagerId(Guid managerId);

        Task PatchSchedule(Schedule schedule);

        Task DeleteScheduleByGameId(Guid gameId);

        Task<Result> FindResultByGameId(Guid gameId);

        Task PatchResult(Result result);

        Task DeleteResultById(Guid gameId);

        Task<List<Result>> FindResultsByManagerId(Guid managerId);

        Task DeleteTeamById(Guid teamId);

    }
}