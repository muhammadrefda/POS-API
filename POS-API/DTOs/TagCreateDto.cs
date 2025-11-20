using System.ComponentModel.DataAnnotations;

namespace POS_API.DTOs
{
    public class TagCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string TagName { get; set; }
    }
}
