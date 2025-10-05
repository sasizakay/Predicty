using Predicty.Repositories;
using Predicty.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Predicty.Models.Dtos;

namespace Predicty.Services
{
    public class UserService
    {
        //CreateUserAsync
        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserDTO> CreateUserAsync(string userName, string passwordHash, string email)
        {
            User newUser = new User
            {
                UserName = userName,
                PasswordHash = passwordHash,
                Email = email,
                CreatedDate = DateTime.UtcNow
            };

            User AddedUser = await _userRepository.AddUserAsync(newUser);
            UserDTO returnUserDTO = new UserDTO
            {
                UserId = newUser.UserId,
                UserName = newUser.UserName,
                Email = newUser.Email,
                CreatedDate = newUser.CreatedDate
            };

            return returnUserDTO;
        }

        public async Task<UserDTO> GetUserByID(int userID)
        {
            return await _userRepository.GetUserByIDAsync(userID);
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
    }
}
