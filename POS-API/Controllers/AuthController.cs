using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS_API.DTOs;
using POS_API.Interfaces;
using System.Security.Claims;

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

        //[Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto req)
        {
            try
            {
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                if (role == "Admin")
                {
                    var result = await _authService.RegisterAsync(req);

                    return Ok(result);
                
                } else
                {
                    return Unauthorized();
                }

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


        [HttpPost("registerByAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterByAdmin(RegisterDto req)
        {
            try
            {
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                if (role == "Admin")
                {
                    var result = await _authService.RegisterAsync(req);

                    return Ok(result);
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
