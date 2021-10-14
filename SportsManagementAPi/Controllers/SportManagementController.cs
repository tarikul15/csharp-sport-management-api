using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SportsManagementAPi.Domain.Models;
using SportsManagementAPi.Domain.Services;
using Microsoft.AspNetCore.JsonPatch;
using Swashbuckle.AspNetCore.Filters;

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

        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TeamResource), StatusCodes.Status200OK)]
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

        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(DeleteTeamResponse), StatusCodes.Status404NotFound)]
        [HttpDelete("deleteTeam/{teamId:guid}")]
        [Authorize]

        public async Task<IActionResult> DeleteTeam([FromRoute] Guid teamId)
        {
            try
            {
                var managerId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "jti")?.Value);
                var deleteResponse = await _sportManagementService.DeleteTeamId(teamId, managerId);

                if (!deleteResponse.Success)
                {
                    return BadRequest(deleteResponse.Message);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PlayerResource), StatusCodes.Status200OK)]
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

        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPatch("patchPlayer/{playerId:guid}")]
        [Authorize]

        public async Task<IActionResult> PatchPlayer([FromRoute] Guid playerId, [FromBody] JsonPatchDocument<PlayerResource> patchPlayer)
        {
            try
            {
                var managerId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "jti")?.Value);
                if (patchPlayer == null)
                {
                    return BadRequest();
                }

                var player = await _sportManagementService.FindPlayerByIdAsync(playerId);

                if (player == null)
                {
                    return NotFound();
                }

                var playerToPatch = _mapper.Map<Player,PlayerResource>(player);

                patchPlayer.ApplyTo(playerToPatch, ModelState);

                TryValidateModel(playerToPatch);

                if (!ModelState.IsValid || playerToPatch.Id != player.Id || playerToPatch.TeamId != player.TeamId || playerToPatch.ManagerId != player.ManagerId)
                {
                    if (playerToPatch.Id != player.Id)
                    {
                        ModelState.AddModelError("Id", "Not Editable");
                    }
                    if (playerToPatch.TeamId != player.TeamId)
                    {
                        ModelState.AddModelError("TeamId", "Not Editable");
                    }
                    if (playerToPatch.ManagerId != player.ManagerId)
                    {
                        ModelState.AddModelError("ManagerId", "Not Editable");
                    }

                    return BadRequest(ModelState);
                }

                player = _mapper.Map<PlayerResource, Player>(playerToPatch, player);
                
                var patchResponse = await _sportManagementService.PatchPlayer(player, managerId);

                return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PlayerResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetPlayersResponse), StatusCodes.Status404NotFound)]
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

        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(DeletePlayerResponse), StatusCodes.Status404NotFound)]
        [HttpDelete("deletePlayer/{playerId:guid}")]
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

                return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ScheduleResource), StatusCodes.Status200OK)]
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

                var scheduleResource = _mapper.Map<Schedule, ScheduleResource>(response.Schedule);
                return Ok(scheduleResource);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ScheduleAndResultResources>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetScheduleIWithResultResponse), StatusCodes.Status404NotFound)]
        [HttpGet("getSchedulesWithResults")]
        [Authorize]

        public async Task<IActionResult> GetSchedulesAndResults()
        {
            try
            {
                var managerId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "jti")?.Value);
                var scheduleWithResultsResponse = await _sportManagementService.GetScheduleWithResultsByManagerId(managerId);

                if (!scheduleWithResultsResponse.Success)
                {
                    return BadRequest(scheduleWithResultsResponse.Message);
                }

                var listOfScheduleResources = _mapper.Map<List<Schedule>, List<ScheduleAndResultResources>>(scheduleWithResultsResponse.Schedules);

                return Ok(listOfScheduleResources);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPatch("patchSchedule/{gameId:guid}")]
        [Authorize]

        public async Task<IActionResult> PatchSchedule([FromRoute] Guid gameId, [FromBody] JsonPatchDocument<ScheduleResource> patchSchedule)
        {
            try
            {
                var managerId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "jti")?.Value);
                if (patchSchedule == null)
                {
                    return BadRequest();
                }

                var schedule = await _sportManagementService.FindScheduleByGameId(gameId);

                if (schedule == null)
                {
                    return NotFound();
                }

                var scheduleToPatch = _mapper.Map<Schedule, ScheduleResource>(schedule);

                patchSchedule.ApplyTo(scheduleToPatch, ModelState);

                TryValidateModel(scheduleToPatch);

                if (!ModelState.IsValid || scheduleToPatch.GameId != schedule.GameId || scheduleToPatch.HomeTeamId != schedule.HomeTeamId ||
                                            scheduleToPatch.HomeTeamName != schedule.HomeTeamName || scheduleToPatch.AwayTeamId != schedule.AwayTeamId 
                                            || scheduleToPatch.AwayTeamName != schedule.AwayTeamName || scheduleToPatch.ManagerId != schedule.ManagerId 
                                            || scheduleToPatch.ScheduledTime != schedule.ScheduledTime)
                {
                    if (scheduleToPatch.GameId != schedule.GameId)
                    {
                        ModelState.AddModelError("GameId", "Not Editable");
                    }
                    if (scheduleToPatch.HomeTeamId != schedule.HomeTeamId && schedule.ScheduledTime < DateTime.Now)
                    {
                        ModelState.AddModelError("HomeTeamId", "HomeTeamId is Not Editable as this is a past game");
                    }
                    if (scheduleToPatch.HomeTeamName != schedule.HomeTeamName && schedule.ScheduledTime < DateTime.Now)
                    {
                        ModelState.AddModelError("HomeTeamName", "HomeTeamName is Not Editable as this is a past game");
                    }
                    if (scheduleToPatch.AwayTeamName != schedule.AwayTeamName && schedule.ScheduledTime < DateTime.Now)
                    {
                        ModelState.AddModelError("AwayTeamName", "AwayTeamName is Not Editable as this is a past game");
                    }
                    if (scheduleToPatch.AwayTeamId != schedule.AwayTeamId && schedule.ScheduledTime < DateTime.Now)
                    {
                        ModelState.AddModelError("AwayTeamId", "AwayTeamId is Not Editable as this is a past game");
                    }
                    if (scheduleToPatch.ScheduledTime != schedule.ScheduledTime && schedule.ScheduledTime < DateTime.Now)
                    {
                        ModelState.AddModelError("ScheduledTime", "ScheduledTime is Not Editable as this is a past game");
                    }
                    if (scheduleToPatch.ManagerId != schedule.ManagerId)
                    {
                        ModelState.AddModelError("ManagerId", "Not Editable");
                    }

                    return BadRequest(ModelState);
                }

                schedule = _mapper.Map<ScheduleResource, Schedule>(scheduleToPatch, schedule);

                var patchscheduleResponse = await _sportManagementService.PatchSchedule(schedule, managerId);

                return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(DeleteScheduleResponse), StatusCodes.Status404NotFound)]
        [HttpDelete("deleteSchedule/{gameId:guid}")]
        [Authorize]

        public async Task<IActionResult> DeleteSchedule([FromRoute] Guid gameId)
        {
            try
            {
                var managerId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "jti")?.Value);
                var deleteResponse = await _sportManagementService.DeleteScheduleByGameId(gameId, managerId);

                if (!deleteResponse.Success)
                {
                    return BadRequest(deleteResponse.Message);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResultResource), StatusCodes.Status200OK)]
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

        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ResultResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetResultsResponse), StatusCodes.Status404NotFound)]
        [HttpGet("getResults")]
        [Authorize]

        public async Task<IActionResult> GetResults()
        {
            try
            {
                var managerId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "jti")?.Value);
                var scheduleWithResultsResponse = await _sportManagementService.FindResultsByManagerId(managerId);

                if (!scheduleWithResultsResponse.Success)
                {
                    return BadRequest(scheduleWithResultsResponse.Message);
                }

                var listOfResults = _mapper.Map<List<Result>, List<ResultResource>>(scheduleWithResultsResponse.Results);

                return Ok(listOfResults);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPatch("patchResult/{gameId:guid}")]
        [Authorize]

        public async Task<IActionResult> PatchResult([FromRoute] Guid gameId, [FromBody] JsonPatchDocument<ResultResource> patchResult)
        {
            try
            {
                var managerId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "jti")?.Value);
                if (patchResult == null)
                {
                    return BadRequest();
                }

                var result = await _sportManagementService.FindResultByGameId(gameId);

                if (result == null)
                {
                    return NotFound();
                }

                var resultToPatch = _mapper.Map<Result, ResultResource>(result);

                patchResult.ApplyTo(resultToPatch, ModelState);

                TryValidateModel(resultToPatch);

                // If model is not valid, return the problem
                if (!ModelState.IsValid || resultToPatch.GameId != result.GameId || resultToPatch.WinnerTeamId != result.WinnerTeamId ||
                                            resultToPatch.LoserTeamId != result.LoserTeamId || resultToPatch.ManagerId != result.ManagerId)
                {
                    if (resultToPatch.GameId != result.GameId)
                    {
                        ModelState.AddModelError("GameId", "Not Editable");
                    }
                    if (resultToPatch.WinnerTeamId != result.WinnerTeamId)
                    {
                        ModelState.AddModelError("WinnerTeamId", "Not Editable");
                    }
                    if (resultToPatch.LoserTeamId != result.LoserTeamId)
                    {
                        ModelState.AddModelError("LoserTeamId", "Not Editable");
                    }
                    if (resultToPatch.ManagerId != result.ManagerId)
                    {
                        ModelState.AddModelError("ManagerId", "Not Editable");
                    }

                    return BadRequest(ModelState);
                }

                // Assign entity changes to original entity retrieved from database
                result = _mapper.Map<ResultResource, Result>(resultToPatch, result);

                var patchResultResponse = await _sportManagementService.PatchResult(result, managerId);

                // If everything was ok, return no content status code to users
                return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(DeleteResultResponse), StatusCodes.Status404NotFound)]
        [HttpDelete("deleteResult/{gameId:guid}")]
        [Authorize]

        public async Task<IActionResult> DeleteResult([FromRoute] Guid gameId)
        {
            try
            {
                var managerId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "jti")?.Value);
                var deleteResponse = await _sportManagementService.DeleteResultByGameId(gameId, managerId);

                if (!deleteResponse.Success)
                {
                    return BadRequest(deleteResponse.Message);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
