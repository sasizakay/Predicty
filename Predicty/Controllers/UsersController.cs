using Predicty.Models.Dtos;
using Predicty.Services;
using Microsoft.AspNetCore.Mvc;

namespace Predicty.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>List<UserDTO></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            List<UserDTO> users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Creates a new user, given user name, e-mail and password
        /// </summary>
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            UserDTO user = await _userService.CreateUserAsync(request.UserName, request.PasswordHash, request.Email);
            return Ok(user);
        }

        [HttpPost("get-user-by-id")]
        public async Task<IActionResult> GetUserByIDAsync(int userID)
        {
            UserDTO user = await _userService.GetUserByID(userID);
            return Ok(user);
        }
        public class CreateUserRequest
        {
            public string UserName { get; set; }
            public string PasswordHash { get; set; }
            public string Email { get; set; }
        }
    }
}
