// File: DTOs/ProductUpdateDto.cs
using System.ComponentModel.DataAnnotations;

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

    // Saat update, pengguna bisa mengubah status aktif produk.
    [Required]
    public bool Active { get; set; }

    // Pengguna juga bisa mengubah daftar tag yang terhubung.
    public List<long> TagIds { get; set; }
}