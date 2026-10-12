using Microsoft.AspNetCore.Mvc;
using TaskTrack.Service.DTOs;
using TaskTrack.Service.Services;

namespace TaskTrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>POST /api/auth/register — Register a new Staff account (Public)</summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var created = await _authService.RegisterStaffAsync(dto);
                return StatusCode(StatusCodes.Status201Created, new
                {
                    message = "Staff account registered successfully.",
                    account = created
                });
            }
            catch (InvalidOperationException ex)
            {
                // Email uniqueness conflict — HTTP 409
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>POST /api/auth/login — Authenticate and receive signed JWT token (Public)</summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.LoginAsync(dto);
                if (result == null)
                {
                    // Invalid credentials — HTTP 401
                    return Unauthorized(new { message = "Invalid email or password." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Login service exception: " + ex.Message,
                    detail = ex.InnerException?.Message
                });
            }
        }
    }
}
