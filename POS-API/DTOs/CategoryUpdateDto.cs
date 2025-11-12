using System.ComponentModel.DataAnnotations;

namespace POS_API.DTOs
{
    public class CategoryUpdateDto
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100)]
        public string Name { get; set; }
    }
}