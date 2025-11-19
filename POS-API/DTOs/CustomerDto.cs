namespace POS_API.DTOs
{
    public class CustomerDto
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; } // Akan dihitung dari DeletedAt
        public DateTime JoinDate { get; set; } // Akan di-mapping dari CreatedAt
    }
}
