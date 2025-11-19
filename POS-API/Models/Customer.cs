namespace POS_API.Models
{
    public class Customer : BaseEntity
    {
        public string FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }

        // 'JoinDate' dari ERD sekarang adalah 'CreatedAt' dari BaseEntity
        // 'Active' dari ERD sekarang dikontrol oleh 'DeletedAt' dari BaseEntity

        public ICollection<Transaction> Transactions { get; set; } =  new List<Transaction>();
    }
}
