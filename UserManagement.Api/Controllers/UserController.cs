using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Solo usuarios autenticados pueden acceder
    public class UserController : ControllerBase
    {
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            return Ok(new { Message = "Perfil del usuario", Email = userEmail });
        }
    }
}
