namespace POS_API.Models
{
    public class TransactionDetail : BaseEntity
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; } //harga produk saat trx
        public decimal SubTotal { get; set; }

        public long TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }

    }
}
