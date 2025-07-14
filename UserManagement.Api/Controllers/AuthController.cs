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
        private readonly LoginAttemptService _loginAttemptService;

        public AuthController(UserService userService, JwtService jwtService, LoginAttemptService loginAttemptService)
        {
            _userService = userService;
            _jwtService = jwtService;
            _loginAttemptService = loginAttemptService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var key = dto.Email + "|" + HttpContext.Connection.RemoteIpAddress?.ToString();

            var user = await _userService.GetUserByEmail(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                _loginAttemptService.RegisterFailedAttempt(key);
                return Unauthorized("Credenciales inválidas");
            }

            _loginAttemptService.ResetAttempts(key);

            var token = _jwtService.GenerateToken(user.Email);
            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var hashedPass = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            await _userService.RegisterUser(dto.Username, dto.Email, hashedPass);
            return Ok(new { Message = "Usuario creado correctamente" });
        }
    }
}
