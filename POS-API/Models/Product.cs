using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS_API.Models
{
    public class Product : BaseEntity
    {
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool Active { get; set; } = true;

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        // Objek navigasi yang akan di-load oleh .Include()
        public Category Category { get; set; }
    }
}
