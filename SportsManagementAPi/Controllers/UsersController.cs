using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsManagementAPi.Domain.Models;
using SportsManagementAPi.Domain.Services;

namespace sportsManagementAPi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IManagerService _userService;

        public UsersController(IManagerService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateManagerRequest userCredentials)
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