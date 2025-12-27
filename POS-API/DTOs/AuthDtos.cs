using System.ComponentModel.DataAnnotations;

namespace POS_API.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty; // Password asli (Plain) cuma sampai sini
        public string Role { get; set; } = "Cashier";
    }

    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}