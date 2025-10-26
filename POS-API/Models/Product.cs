using System.ComponentModel.DataAnnotations;

namespace POS_API.Models
{
    public class Product : BaseEntity
    {
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool Active { get; set; } = true;

        // 1 to m
        public long CategoryId { get; set; }
        public Category? Category { get; set; }

        // m to m
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
