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
            await _unitOfWork.CompleteAsync();

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
            await _unitOfWork.CompleteAsync();

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
            await _unitOfWork.CompleteAsync();

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
                return new DeletePlayerResponse(false, "Can update the Result of a Game as it is created by another manager.", null);
            }

            if (existingPlayer.ManagerId != managerId)
            {
                return new DeletePlayerResponse(false, "This player is not managed by the manager requesting this action.", null);
            }

            await _sportManagementRepository.DeletePlayerById(existingPlayer.Id);
            await _unitOfWork.CompleteAsync();

            return new DeletePlayerResponse(true, "Delete Successful", null);
        }

    }
}