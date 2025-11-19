using System.ComponentModel.DataAnnotations;

namespace POS_API.DTOs
{
    // DTO untuk membuat Tag baru
    public class TagCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string TagName { get; set; }
    }
}