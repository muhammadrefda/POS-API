namespace POS_API.DTOs
{
    public class TransactionCreateDto
    {
        public long CustomerId { get; set; }
        public string PaymentMethod { get; set; } // cash, QRIS, dll

        //List barang yg akan dibeli
        public List<TransactionDetailCreateDto> Details { get; set; }
    }

    public class TransactionDetailCreateDto
    {
        public long ProductId { get; set; }
        public int Qty { get; set; }
    }
}
