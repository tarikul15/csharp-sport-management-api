using System.Threading.Tasks;
using SportsManagementAPi.Domain.Models;
using SportsManagementAPi.Domain.Repositories;
using SportsManagementAPi.Domain.Services;
using SportsManagementAPi.Domain.Security;

namespace SportsManagementAPi.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public ManagerService(IManagerRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<CreateManagerResponse> CreateUserAsync(Manager user)
        {
            var existingUser = await _userRepository.FindByEmailAsync(user.Email);
            if(existingUser != null)
            {
                return new CreateManagerResponse(false, "Email already in use.", null);
            } 

            user.Password = _passwordHasher.HashPassword(user.Password);

            await _userRepository.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return new CreateManagerResponse(true, null, user);
        }

        public async Task<Manager> FindByEmailAsync(string email)
        {
            return await _userRepository.FindByEmailAsync(email);
        }
    }
}