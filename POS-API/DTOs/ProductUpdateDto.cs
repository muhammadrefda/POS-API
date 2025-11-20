using System.ComponentModel.DataAnnotations;

namespace POS_API.DTOs
{
    public class ProductUpdateDto
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
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Required]
        public bool Active { get; set; }
        public List<long> TagIds { get; set; }

    }
}
