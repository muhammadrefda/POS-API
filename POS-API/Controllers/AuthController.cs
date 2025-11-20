using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS_API.DTOs;
using POS_API.Interfaces;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto req)
        {
            try
            {
                var result = await _authService.RegisterAsync(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto req)
        {
            try
            {
                var token = await _authService.LoginAsync(req);
                return Ok(new {token});
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
