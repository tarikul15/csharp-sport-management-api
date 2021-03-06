using System;
using System.Collections.Generic;
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
            await _context.SaveChangesAsync();
        }

        public async Task<Team> FindTeamByName(string name)
        {
            return await _context.Teams.SingleOrDefaultAsync(u => u.Name == name);

        }

        public async Task AddPlayerAsync(Player player)
        {
            await _context.Players.AddAsync(player);
            await _context.SaveChangesAsync();
        }

        public async Task<Player> FindPlayerByName(string name)
        {
            return await _context.Players.SingleOrDefaultAsync(p => p.Name == name);
        }

        public async Task AddScheduleAsync(Schedule schedule)
        {
            await _context.Schedules.AddAsync(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task<Schedule> FindScheduleByTimeAndTeams(DateTime time, Guid homeTeamId, Guid awayTeamId)
        {
            return await _context.Schedules.SingleOrDefaultAsync(s=>s.ScheduledTime == time 
                                                                    && s.HomeTeamId == homeTeamId 
                                                                    && s.AwayTeamId == awayTeamId);
        }

        public async Task AddResultAsync(Result result)
        {
            await _context.Results.AddAsync(result);
            await _context.SaveChangesAsync();
        }

        public async Task<Schedule> FindScheduleByGameId(Guid gameId)
        {
            return await _context.Schedules.SingleOrDefaultAsync(s => s.GameId == gameId);
        }

        public async Task<Team> FindTeamById(Guid teamId)
        {
            return await _context.Teams.SingleOrDefaultAsync(t => t.Id == teamId);
        }

        public async Task<List<Player>> GetPlayersByManagerId(Guid managerId)
        {
            return await _context.Players.Where(p => p.ManagerId == managerId).ToListAsync();
        }

        public async Task DeletePlayerById(Guid id)
        {
            var playerToDelete = await _context.Players.SingleOrDefaultAsync(p => p.Id == id);
             _context.Players.Remove(playerToDelete);
             await _context.SaveChangesAsync();
        }

        public async Task<Player> FindPlayerById(Guid playerId)
        {
            return await _context.Players.SingleOrDefaultAsync(p => p.Id == playerId);
        }

        public async Task PatchPlayer(Player player)
        {
            _context.Entry(player).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task<List<Schedule>> GetScheduleWithResultsByManagerId(Guid managerId)
        {
            return await _context.Schedules
                .Include(s => s.Result)
                .Where(s => s.ManagerId == managerId).ToListAsync();
        }

        public async Task PatchSchedule(Schedule schedule)
        {
            _context.Entry(schedule).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteScheduleByGameId(Guid gameId)
        {
            var scheduleToDelete = await _context.Schedules.SingleOrDefaultAsync(s => s.GameId == gameId);
            _context.Schedules.Remove(scheduleToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<Result> FindResultByGameId(Guid gameId)
        {
            return await _context.Results.SingleOrDefaultAsync(r => r.GameId == gameId);
        }

        public async Task PatchResult(Result result)
        {
            _context.Entry(result).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteResultById(Guid gameId)
        {
            var resultToDelete = await _context.Results.SingleOrDefaultAsync(r => r.GameId == gameId);
            _context.Results.Remove(resultToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Result>> FindResultsByManagerId(Guid managerId)
        {
            return await _context.Results.Where(r => r.ManagerId == managerId).ToListAsync();
        }

        public async Task DeleteTeamById(Guid teamId)
        {
            var teamToDelete = await _context.Teams.SingleOrDefaultAsync(t => t.Id == teamId);
            _context.Teams.Remove(teamToDelete);
            await _context.SaveChangesAsync();
        }
    }
}