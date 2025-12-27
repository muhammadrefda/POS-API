using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS_API.Models
{
    public class Product : BaseEntity
    {
        [Display(Name = "Product categoryName")]
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool Active { get; set; } = true;




        [ForeignKey("Category")]
        public long CategoryId { get; set; }

        // Objek navigasi yang akan di-load oleh .Include()
        public Category? Category { get; set; }

        public ICollection<ProductTag> ProductTags { get; set; } = new List<ProductTag>();
    }
}
