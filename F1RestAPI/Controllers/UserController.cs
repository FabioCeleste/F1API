using F1RestAPI.Contracts.Users;
using F1RestAPI.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DriversApi.Controllers

{
    [Authorize]
    [ApiController]
    [Route("api/users")]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(CreateUserRequest createUserRequest)
        {
            try
            {
                var isUserCreated = await _userService.CreateUser(createUserRequest);

                return Ok(new { created = isUserCreated });
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(CreateUserRequest loginRequest)
        {
            try
            {
                var token = await _userService.Login(loginRequest);

                if (token.IsError)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }

                return Ok(new { token = token.Value });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}