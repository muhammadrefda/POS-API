using System.ComponentModel.DataAnnotations;

namespace POS_API.DTOs
{
    // DTO untuk memperbarui Tag
    public class TagUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string TagName { get; set; }
    }
}