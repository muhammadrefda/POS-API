namespace POS_API.DTOs
{
    public class ProductDto
    {
        public long Id { get; set; }
        public string  ProductName { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool Active { get; set; }
        public string CategoryName { get; set; }
        public List<string> Tags { get; set; }

    }
}
