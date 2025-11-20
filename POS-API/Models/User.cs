using System.ComponentModel.DataAnnotations;

namespace POS_API.Models
{
    public class User
    {
        public long Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Cashier"; // default role
    }
}
