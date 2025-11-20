using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using POS_API.Data;
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
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService (ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(RegisterDto request)
        {
            //cek username ada ga?
            if (await _context.Users.AnyAsync( u => u.Username == request.Username))
            {
                throw new Exception("Username already register");
            }

            //encrypt password (hashing)
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            //simpan ke db
            var user = new User { 
                Username = request.Username,
                PasswordHash = passwordHash,
                Role = request.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "Registrasi Berhasil!";
        }

        public async Task<string> LoginAsync(LoginDto request)
        {
            // cari user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
            {
                throw new Exception("Username or Password is wrong");
            }

            //verifikasi password (hash vs plain)
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new Exception("Username or Password is wrong");
            }

            //generate token
            return CreateToken(user);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("id", user.Id.ToString()),
            };

            var secretKey = _configuration.GetSection("JwtSettings:SecretKey").Value!;


            var normalBase64 = Base64UrlToBase64(secretKey!);

            var keyBytes = Convert.FromBase64String(normalBase64);

            var key = new SymmetricSecurityKey(keyBytes);

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

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
    }
}
