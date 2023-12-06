using authentication_jwt_dotnet.DTOs;
using authentication_jwt_dotnet.Models;
using authentication_jwt_dotnet.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
                Name = registerModel.Name,
                Roles = new List<Role>()
                {
                    new Role() { Name = "user" }
                }
            };

            await _userService.AddAsync(newUser);

            return Ok("The user has been added");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel loginModel)
        {
            User? user = await _userService.FindAsync(_ => _.Email == loginModel.Email);
            if (user == null)
                return BadRequest("The user does not exist");

            if (!BCrypt.Net.BCrypt.Verify(loginModel.Password, user.PasswordHashed))
                return BadRequest("Wrong password");

            var token = _tokenService.CreateToken(user);

            return Ok(new { token });
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateName([FromQuery]string name)
        {
            string? id = User.FindFirst(ClaimTypes.Name)?.Value;
            if (id == null)
                return Unauthorized("Unauthorized");

            User? user = await _userService.FindAsync(id);
            if (user == null)
                return Unauthorized("Unauthorized");

            user.Name = name;
            await _userService.UpdateAsync(user);

            return Ok("The user has been updated");
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute]string id)
        {
            await _userService.DeleteAsync(id);

            return Ok("The user has been deleted");
        }
    }
}
