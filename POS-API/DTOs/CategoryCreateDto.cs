using System.ComponentModel.DataAnnotations;

namespace POS_API.DTOs
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100)]
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true!;
    }
}