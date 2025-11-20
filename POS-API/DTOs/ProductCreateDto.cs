using System.ComponentModel.DataAnnotations;

namespace POS_API.DTOs
{
    public class ProductCreateDto
    {
        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }

        [Required]
        [Range(1, long.MaxValue)]
        public long CategoryId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, 1000)]
        public int Stock { get; set; }
        
        public List<long> TagIds { get; set; }
    }
}
