using System;
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
