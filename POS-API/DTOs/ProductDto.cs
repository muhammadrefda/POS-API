namespace POS_API.DTOs
{
    public class ProductDto
    {
        public long Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool Active { get; set; }

        // --- Data dari Relasi (Enrichment) ---

        // Kita tampilkan nama kategori, bukan hanya ID-nya.
        // Ini jauh lebih berguna untuk frontend.
        public string CategoryName { get; set; }

        // Sebuah produk bisa punya banyak tag.
        // Kita kirim daftar nama tag-nya.
        public List<string> Tags { get; set; }
    }
}
