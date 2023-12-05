using authentication_jwt_dotnet.DTOs;
using authentication_jwt_dotnet.Models;
using authentication_jwt_dotnet.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace authentication_jwt_dotnet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        /**
         * The responses of the APIs should be more organized.
         * However, just for the simplicity of the project,
         * they return directly a message
         */

        private readonly IUserService _userService;
        private readonly IJwtTokenService _tokenService;

        public UserController(IJwtTokenService tokenService, IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel registerModel)
        {
            User? user = await _userService.FindAsync(_ => _.Email == registerModel.Email);
            if (user != null)
                return BadRequest("The user already exist");

            User newUser = new User()
            {
                Email = registerModel.Email,
                PasswordHashed = BCrypt.Net.BCrypt.HashPassword(registerModel.Password),
                Name = registerModel.Name
            };

            await _userService.AddAsync(newUser);

            return Ok("The user has been added");
        }
    }
}
