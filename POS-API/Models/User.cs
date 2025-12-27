using System.ComponentModel.DataAnnotations;

namespace POS_API.Models
{
    public class User
    {
        public long Id { get; set; } 
        // Tipe Long (BigInt)

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty; // Ingat: Bukan password asli

        [Required]
        public string Role { get; set; } = "Cashier"; // Default role
    }
}