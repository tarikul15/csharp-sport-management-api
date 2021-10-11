using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SportsManagementAPi.Domain.Models;
using SportsManagementAPi.Domain.Services;

namespace SportsManagementAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportManagementController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISportManagementService _sportManagementService;


        public SportManagementController(ISportManagementService sportManagementService, IMapper mapper)
        {
            _sportManagementService = sportManagementService;
            _mapper = mapper;
        }

        [HttpPost("createTeam")]
        [Authorize]

        public async Task<IActionResult> CreateTeam([FromBody]CreateTeamRequest teamRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var team = _mapper.Map<CreateTeamRequest, Team>(teamRequest);

                var id = User.Claims.FirstOrDefault(c => c.Type == "jti")?.Value;
                team.ManagerId = Guid.Parse(id);

                var response = await _sportManagementService.CreateTeamAsync(team);
                if (!response.Success)
                {
                    return BadRequest(response.Message);
                }

                var teamResource = _mapper.Map<Team, TeamResource>(response.Team);
                return Ok(teamResource);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("createPlayer")]
        [Authorize]

        public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerRequest playerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var player = _mapper.Map<CreatePlayerRequest, Player>(playerRequest);
                var team = await _sportManagementService.FindTeamByNameAsync(playerRequest.TeamName);
                if (team == null)
                {
                    var createPlayerErrorResponse = new CreatePlayerResponse(false, "can't find this team in the db", null);
                    return BadRequest(createPlayerErrorResponse.Message);
                }

                player.TeamId = team.Id;
                player.ManagerId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "jti")?.Value);

                var response = await _sportManagementService.CreatePlayerAsync(player);
                if (!response.Success)
                {
                    return BadRequest(response.Message);
                }

                var playerResource = _mapper.Map<Player, PlayerResource>(response.Player);
                return Ok(playerResource);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet("getPlayers")]
        [Authorize]

        public async Task<IActionResult> GetPlayers()
        {
            try
            {
                var managerId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "jti")?.Value);
                var playerResponse = await _sportManagementService.GetPlayersByManagerId(managerId);
                
                if (!playerResponse.Success)
                {
                    return BadRequest(playerResponse.Message);
                }

                var listOfPlayerResources = _mapper.Map<List<Player>, List<PlayerResource>>(playerResponse.Players);

                return Ok(listOfPlayerResources);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpDelete("deletePlayer/{playerId}")]
        [Authorize]

        public async Task<IActionResult> DeletePlayer([FromRoute] Guid playerId)
        {
            try
            {
                var managerId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "jti")?.Value);
                var deleteResponse = await _sportManagementService.DeletePlayerById(playerId,managerId);

                if (!deleteResponse.Success)
                {
                    return BadRequest(deleteResponse.Message);
                }

                return Ok(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [HttpPost("schedule")]
        [Authorize]

        public async Task<IActionResult> CreateSchedule([FromBody] CreateScheduleRequest playerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var schedule = _mapper.Map<CreateScheduleRequest, Schedule>(playerRequest);
                var homeTeam = await _sportManagementService.FindTeamByNameAsync(schedule.HomeTeamName);
                var awayTeam = await _sportManagementService.FindTeamByNameAsync(schedule.AwayTeamName);

                schedule.HomeTeamId = homeTeam.Id;
                schedule.AwayTeamId = awayTeam.Id;

                schedule.ManagerId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "jti")?.Value);

                var response = await _sportManagementService.CreateScheduleAsync(schedule);
                if (!response.Success)
                {
                    return BadRequest(response.Message);
                }

                var playerResource = _mapper.Map<Schedule, ScheduleResource>(response.Schedule);
                return Ok(playerResource);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("result")]
        [Authorize]
        public async Task<IActionResult> CreateResult([FromBody] CreateResultRequest resultRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = _mapper.Map<CreateResultRequest, Result>(resultRequest);

                var winnerTeam = await _sportManagementService.FindTeamByNameAsync(resultRequest.WinnerTeamName);
                var loserTeam = await _sportManagementService.FindTeamByNameAsync(resultRequest.LoserTeamName);

                result.WinnerTeamId = winnerTeam.Id;
                result.LoserTeamId = loserTeam.Id;
                result.ManagerId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "jti")?.Value);

                var response = await _sportManagementService.CreateResultAsync(result);
                if (!response.Success)
                {
                    return BadRequest(response.Message);
                }

                var playerResource = _mapper.Map<Result, ResultResource>(response.Result);
                return Ok(playerResource);
            }
            catch (Exception ex)
            {
                throw;
            }
        }




        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
