using System.ComponentModel.DataAnnotations;

namespace POS_API.DTOs
{
    public class CustomerUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}