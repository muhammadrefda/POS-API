namespace POS_API.DTOs
{
    // Ini Header (Data Nota)
    public class TransactionCreateDto
    {
        public long CustomerId { get; set; }
        public string PaymentMethod { get; set; } // Cash, QRIS, dll

        // List barang yang dibeli
        public List<TransactionDetailCreateDto> Details { get; set; }
    }

    // Ini Detail (Barang per item)
    public class TransactionDetailCreateDto
    {
        public long ProductId { get; set; }
        public int Qty { get; set; }
    }
}