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

        public DbSet<ProductTag> ProductTags { get; set; }

        public DbSet<User> Users { get; set; }

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
                CategoryId = foodCategory.Id,
            };

            modelBuilder.Entity<Product>().HasData(product);

            var customer = new Customer
            {
                Id = 1,
                FullName = "Budi Santoso",
                Email = "budi@example.com",
                //JoinDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            modelBuilder.Entity<Customer>().HasData(customer);

            // 1. Tentukan Composite Key untuk tabel ProductTag
            modelBuilder.Entity<ProductTag>()
                .HasKey(pt => new { pt.ProductId, pt.TagId });

            // 2. Konfigurasi relasi dari Product ke ProductTag
            modelBuilder.Entity<ProductTag>()
                .HasOne(pt => pt.Product)
                .WithMany(p => p.ProductTags) // Mereferensi ICollection di Product.cs
                .HasForeignKey(pt => pt.ProductId);

            // 3. Konfigurasi relasi dari Tag ke ProductTag
            modelBuilder.Entity<ProductTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.ProductTags) // Mereferensi ICollection di Tag.cs
                .HasForeignKey(pt => pt.TagId);
        }
    }

}

