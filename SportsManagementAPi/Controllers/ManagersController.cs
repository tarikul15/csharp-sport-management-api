using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsManagementAPi.Domain.Models;
using SportsManagementAPi.Domain.Services;
using Swashbuckle.AspNetCore.Filters;

namespace sportsManagementAPi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ManagersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IManagerService _userService;

        public ManagersController(IManagerService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ManagerResource), StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> CreateManagerAsync([FromBody] CreateManagerRequest userCredentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _mapper.Map<CreateManagerRequest, Manager>(userCredentials);

                var response = await _userService.CreateUserAsync(user);
                if (!response.Success)
                {
                    return BadRequest(response.Message);
                }

                var userResource = _mapper.Map<Manager, ManagerResource>(response.User);
                return Ok(userResource);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}