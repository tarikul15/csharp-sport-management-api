using System;
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

        public SportManagementService(ISportManagementRepository sportManagementRepository)
        {
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

            return new CreateTeamResponse(true, null, team);
        }

        public async Task<Team> FindTeamByNameAsync(string name)
        {
            return await _sportManagementRepository.FindTeamByName(name);
        }

        public async Task<CreatePlayerResponse> CreatePlayerAsync(Player player)
        {
            var existingPlayer = await _sportManagementRepository.FindPlayerByName(player.Name);
            var existingTeam = await _sportManagementRepository.FindTeamById(player.TeamId);
            if (existingPlayer != null)
            {
                return new CreatePlayerResponse(false, "Name already in use.", null);
            }

            if (existingTeam.ManagerId != player.ManagerId)
            {
                return new CreatePlayerResponse(false, "This player's team is under a different manager.", null);
            }

            await _sportManagementRepository.AddPlayerAsync(player);

            return new CreatePlayerResponse(true, null, player);
        }

        public async Task<Player> FindPlayerByNameAsync(string name)
        {
            return await _sportManagementRepository.FindPlayerByName(name);
        }

        public async Task<CreateScheduleResponse> CreateScheduleAsync(Schedule schedule)
        {
            var homeTeam = await _sportManagementRepository.FindTeamByName(schedule.HomeTeamName);
            
            if (homeTeam.ManagerId != schedule.ManagerId)
            {
                return new CreateScheduleResponse(false, "The Home Team is not managed by this Manager.", null);
            }

            // find schedules by the manager's team ( home team) as home team
            var managedTeamAsHomeSchedule = await _sportManagementRepository.FindScheduleByTimeAndTeams(schedule.ScheduledTime, schedule.HomeTeamId, schedule.AwayTeamId);

            var managedTeamAsAwaySchedule = await _sportManagementRepository.FindScheduleByTimeAndTeams(schedule.ScheduledTime, schedule.AwayTeamId,schedule.HomeTeamId);

            if (managedTeamAsHomeSchedule != null || managedTeamAsAwaySchedule != null)
            {
                if (managedTeamAsHomeSchedule != null)
                {
                    if ((schedule.ScheduledTime - managedTeamAsHomeSchedule.ScheduledTime).TotalHours <= 24)
                    {
                        return new CreateScheduleResponse(false, "There is another game with these two teams in the last 24hrs.", null);
                    }
                }
                else
                {
                    if ((schedule.ScheduledTime - managedTeamAsAwaySchedule.ScheduledTime).TotalHours <= 24)
                    {
                        return new CreateScheduleResponse(false, "There is another game with these two teams in the last 24hrs.", null);
                    }

                }
            }

            await _sportManagementRepository.AddScheduleAsync(schedule);

            return new CreateScheduleResponse(true, null, schedule);

        }

        public async Task<CreateResultResponse> CreateResultAsync(Result result)
        {
            var existingSchedule = await _sportManagementRepository.FindScheduleByGameId(result.GameId);
            if (existingSchedule.ManagerId != result.ManagerId)
            {
                return new CreateResultResponse(false, "Can update the Result of a Game as it is created by another manager.", null);
            }

            await _sportManagementRepository.AddResultAsync(result);

            return new CreateResultResponse(true, null, result);
        }

        public async Task<GetPlayersResponse> GetPlayersByManagerId(Guid managerId)
        {
            var playerList = await _sportManagementRepository.GetPlayersByManagerId(managerId);

            return new GetPlayersResponse(true, null, playerList);
        }

        public async Task<DeletePlayerResponse> DeletePlayerById(Guid playerId, Guid managerId)
        {
            var existingPlayer = await _sportManagementRepository.FindPlayerById(playerId);
            if (existingPlayer == null)
            {
                return new DeletePlayerResponse(false, "Player Not Found.", null);
            }

            if (existingPlayer.ManagerId != managerId)
            {
                return new DeletePlayerResponse(false, "This player is not managed by the manager requesting this action.", null);
            }

            await _sportManagementRepository.DeletePlayerById(existingPlayer.Id);

            return new DeletePlayerResponse(true, "Delete Successful", null);
        }

        public async Task<Player> FindPlayerByIdAsync(Guid id)
        {
            return await _sportManagementRepository.FindPlayerById(id);
        }

        public async Task<PatchPlayerResponse> PatchPlayer(Player player, Guid managerId)
        {
            if (player.ManagerId != managerId)
            {
                return new PatchPlayerResponse(false, "This Player is not managed by the requesting manager", null);
            }

            await _sportManagementRepository.PatchPlayer(player);
            return new PatchPlayerResponse(true, "Patch Successful", null);
        }

        public async Task<GetScheduleIWithResultResponse> GetScheduleWithResultsByManagerId(Guid managerId)
        {
            var scheduleList = await _sportManagementRepository.GetScheduleWithResultsByManagerId(managerId);
            return new GetScheduleIWithResultResponse(true, "", scheduleList);
        }

        public async Task<Schedule> FindScheduleByGameId(Guid gameId)
        {
            return await _sportManagementRepository.FindScheduleByGameId(gameId);

        }

        public async Task<PatchScheduleResponse> PatchSchedule(Schedule schedule, Guid managerId)
        {
            if (schedule.ManagerId != managerId)
            {
                return new PatchScheduleResponse(false, "The Requesting manager didn't create this game", null);
            }

            await _sportManagementRepository.PatchSchedule(schedule);
            return new PatchScheduleResponse(true, "patch successful", null);

        }

        public async Task<DeleteScheduleResponse> DeleteScheduleByGameId(Guid gameId, Guid managerId)
        {
            var existingSchedule = await _sportManagementRepository.FindScheduleByGameId(gameId);

            if (existingSchedule == null)
            {
                return new DeleteScheduleResponse(false, "Schedule not found.", null);
            }

            if (existingSchedule.ManagerId != managerId)
            {
                return new DeleteScheduleResponse(false, "This game is not created by the requesting manager.", null);
            }

            await _sportManagementRepository.DeleteScheduleByGameId(gameId);
            return new DeleteScheduleResponse(true, "delete successful", null);
        }

        public async Task<Result> FindResultByGameId(Guid gameId)
        {
            return await _sportManagementRepository.FindResultByGameId(gameId);
        }

        public async Task<PatchResultResponse> PatchResult(Result result, Guid managerId)
        {

            if (result.ManagerId != managerId)
            {
                return new PatchResultResponse(false, "The Requesting manager didn't create this game", null);
            }

            await _sportManagementRepository.PatchResult(result);
            return new PatchResultResponse(true, "Patch Successful", null);
        }

        public async Task<DeleteResultResponse> DeleteResultByGameId(Guid gameId, Guid managerId)
        {
            var existingResult = await _sportManagementRepository.FindResultByGameId(gameId);
            if (existingResult == null)
            {
                return new DeleteResultResponse(false, "Result Not Found.", null);
            }

            if (existingResult.ManagerId != managerId)
            {
                return new DeleteResultResponse(false, "This game is not created by the manager requesting this action.", null);
            }

            await _sportManagementRepository.DeleteResultById(gameId);

            return new DeleteResultResponse(true, "Delete Successful", null);

        }

        public async Task<GetResultsResponse> FindResultsByManagerId(Guid managerId)
        {
            var resultList = await _sportManagementRepository.FindResultsByManagerId(managerId);
            return new GetResultsResponse(true, "", resultList);
        }

    }
}