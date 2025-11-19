using System.ComponentModel.DataAnnotations;

namespace POS_API.Models
{
    // Tag juga mewarisi BaseEntity, sama seperti Product & Category
    public class Tag : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string TagName { get; set; }

        // Navigation property untuk relasi Many-to-Many
        public ICollection<ProductTag> ProductTags { get; set; } = new List<ProductTag>();
    }
}