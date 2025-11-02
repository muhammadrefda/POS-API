using POS_API.Models;
using Microsoft.EntityFrameworkCore;


namespace POS_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> o) : base(o) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionDetail> TransactionDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var foodCategory = new Category
            {
                Id = 1,
                CategoryName = "Makanan ringan",
                CreatedAt = DateTime.UtcNow,
            };

            modelBuilder.Entity<Category>().HasData(foodCategory);

            var bestSellerTag = new Tag
            {
                Id = 1,
                TagName = "Best Seller",
                CreatedAt = DateTime.UtcNow
            };

            modelBuilder.Entity<Tag>().HasData(bestSellerTag);

            var product = new Product
            {
                Id = 1,
                ProductName = "Keripik Kentang Original",
                Price = 15000,
                Stock = 100,
                CategoryId = Convert.ToInt32(foodCategory.Id),
            };

            modelBuilder.Entity<Product>().HasData(product);

            var customer = new Customer
            {
                Id = 1,
                FullName = "Budi Santoso",
                Email = "budi@example.com",
                JoinDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            modelBuilder.Entity<Customer>().HasData(customer);
        }
    }

}

