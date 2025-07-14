using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Data;
using UserManagement.Api.Services;
using UserManagement.Api.DTOs;

namespace UserManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtService _jwtService;

        public AuthController(UserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userService.GetUserByEmail(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = _jwtService.GenerateToken(user.Email);
            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var hashedPass = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            await _userService.RegisterUser(dto.Username, dto.Email, hashedPass);
            return Ok(new { Message = "User created" });
        }
    }
}
