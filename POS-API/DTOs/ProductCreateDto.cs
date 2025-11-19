// File: DTOs/ProductCreateDto.cs
using System.ComponentModel.DataAnnotations;

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
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    // Saat membuat produk, frontend akan mengirimkan
    // daftar ID dari tag yang dipilih.

    public List<long> TagIds { get; set; } = new List<long>();
}