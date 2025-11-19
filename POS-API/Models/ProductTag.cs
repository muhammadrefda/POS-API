namespace POS_API.Models
{
    // Ini adalah tabel "jembatan" (Junction Table)
    // TIDAK mewarisi BaseEntity
    public class ProductTag
    {
        public long ProductId { get; set; }
        public Product Product { get; set; }

        public long TagId { get; set; }
        public Tag Tag { get; set; }
    }
}