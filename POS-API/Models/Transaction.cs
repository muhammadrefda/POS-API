namespace POS_API.Models
{
    public class Transaction : BaseEntity
    {
        public DateTime TransactionDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public long CreatedBy { get; set; } // ID Kasir yg bertugas

        //1 to m
        public long CustomerId { get; set; }
        public Customer Customer { get; set; }


        public ICollection<TransactionDetail> TransactionDetail { get; set; } = new List<TransactionDetail>();

    }
}
