namespace POS_API.Models
{
    public class Customer : BaseEntity
    {
        public string FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; } = true;
        public DateTime JoinDate { get; set; }

        public ICollection<Transaction> Transactions { get; set; } =  new List<Transaction>();
    }
}
