using Microsoft.AspNetCore.Mvc;
using FUNewsManagement.Models.DTOs;
using FUNewsManagement.Services.Interfaces;

namespace FUNewsManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];
            var jwtSecret = _configuration["Jwt:SecretKey"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            var result = _authService.Authenticate(request, adminEmail, adminPassword, jwtSecret, jwtIssuer, jwtAudience);
            if (result == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }
            return Ok(result);
        }
    }
}
