using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS_API.DTOs;
using POS_API.Helpers;
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

        // ==========================================
        // 1. PUBLIC AREA (Bisa diakses siapa saja)
        // ==========================================

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto req)
        {
            try
            {
                var token = await _authService.LoginAsync(req);
                var response = new ApiResponse<object>(new { token }, "Login successful");
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Kita pakai constructor kedua: ApiResponse(string errorMessage)
                return Unauthorized(new ApiResponse<object>(ex.Message));
            }
        }

        // ==========================================
        // 2. RESTRICTED AREA (Hanya Admin)
        // ==========================================

        // Endpoint ini dipakai Admin untuk mendaftarkan Kasir/Admin baru
        [HttpPost("register")]
        //[Authorize(Roles = "Admin")] // <--- Gembok Otomatis (Gak perlu cek IF Role == Admin lagi)
        public async Task<IActionResult> Register(RegisterDto req)
        {
            try
            {
                var message = await _authService.RegisterAsync(req);
                // WRAPPER SUKSES: Data null (atau string), Message dari service
                return Ok(new ApiResponse<string>(null, message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message));
            }
        }

        // Melihat semua user (Admin & Kasir)
        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _authService.GetAllUsersAsync();

                // WRAPPER SUKSES: Data berupa List UserDto
                // Catatan: Belum pakai ApiPagedResponse karena belum ada logic paging di Repo
                return Ok(new ApiResponse<IEnumerable<UserDto>>(users, "Users retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message));
            }
        }

        // Menghapus user (Misal: Kasir resign)
        [HttpDelete("users/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            try
            {
                var success = await _authService.DeleteUserAsync(id);
                if (!success)
                {
                    return NotFound(new ApiResponse<object>("User not found"));
                }

                return Ok(new ApiResponse<object>(null, "User deleted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message));
            }
        }

        // Reset password (Misal: Kasir lupa password)
        [HttpPut("users/{id}/reset-password")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetPassword(long id, [FromBody] ResetPasswordDto req)
        {
            try
            {
                var success = await _authService.ResetPasswordAsync(id, req.NewPassword);
                if (!success)
                {
                    return NotFound(new ApiResponse<object>("User not found"));
                }

                return Ok(new ApiResponse<object>(null, "Password reset successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message));
            }
        }
    }
}