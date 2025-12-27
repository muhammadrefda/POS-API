using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using POS_API.Data;
using POS_API.DTOs;
using POS_API.Interfaces;
using POS_API.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace POS_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration; // Buat baca appsettings

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(RegisterDto request)
        {
            // 1. Cek apakah username sudah ada?
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                throw new Exception("Username sudah terdaftar.");
            }

            // 2. Encrypt Password (Hashing)
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 3. Simpan ke DB
            var user = new User
            {
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
            // 1. Cari user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
            {
                throw new Exception("Username atau Password salah.");
            }

            // 2. Verifikasi Password (Hash vs Plain)
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new Exception("Username atau Password salah.");
            }

            // 3. Generate Token
            return CreateToken(user);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("id", user.Id.ToString()) // Penting buat transaksi nanti!
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("JwtSettings:SecretKey").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}