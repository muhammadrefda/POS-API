using Microsoft.IdentityModel.Tokens;
using POS_API.DTOs;
using POS_API.Interfaces;
using POS_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace POS_API.Services
{
    public class AuthService : IAuthService
    {
        // HAPUS _context, karena kita sudah pakai _repository
        private readonly IConfiguration _configuration;
        private readonly IAuthRepository _repository;

        // Hapus ApplicationDbContext dari constructor
        public AuthService(IConfiguration configuration, IAuthRepository repository)
        {
            _configuration = configuration;
            _repository = repository;
        }

        public async Task<string> RegisterAsync(RegisterDto request)
        {
            // Cek username via Repository
            if (await _repository.UserExistsAsync(request.Username))
            {
                throw new Exception("Username already registered");
            }

            // Encrypt password (hashing)
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Simpan ke db via Repository
            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                Role = request.Role
            };

            await _repository.CreateUserAsync(user);

            return "Registration Successful!";
        }

        public async Task<string> LoginAsync(LoginDto request)
        {
            // Cari user via Repository
            var user = await _repository.GetUserByUsernameAsync(request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new Exception("Username or Password is wrong");
            }

            // Generate token
            return CreateToken(user);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                
                // PERBAIKAN PENTING:
                // Gunakan ClaimTypes.Role agar [Authorize(Roles = "...")] di controller jalan otomatis
                new Claim(ClaimTypes.Role, user.Role), 
                
                // Gunakan "Id" (Huruf besar) agar konsisten dengan controller User.FindFirst("Id")
                new Claim("Id", user.Id.ToString()),
            };

            // var secretKey = _configuration.GetSection("JwtSettings:SecretKey").Value!;

            //var normalBase64 = Base64UrlToBase64(secretKey!);
            //var keyBytes = Convert.FromBase64String(normalBase64);
            //var key = new SymmetricSecurityKey(keyBytes);

            //            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var key = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!)
              );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string Base64UrlToBase64(string base64url)
        {
            string output = base64url.Replace('-', '+').Replace('_', '/');
            switch (output.Length % 4)
            {
                case 2: output += "=="; break;
                case 3: output += "="; break;
            }
            return output;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _repository.GetAllUsersAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Role = u.Role
            });
        }

        public async Task<bool> DeleteUserAsync(long id)
        {
            var user = await _repository.GetUserByIdAsync(id);
            if (user == null) return false;

            await _repository.DeleteUserAsync(user);
            return true;
        }

        public async Task<bool> ResetPasswordAsync(long id, string newPassword)
        {
            var user = await _repository.GetUserByIdAsync(id);
            if (user == null) return false;

            // Penting: Hash Password Baru!
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.PasswordHash = passwordHash;

            await _repository.UpdateUserAsync(user);
            return true;
        }
    }
}