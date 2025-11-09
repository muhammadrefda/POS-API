using Microsoft.EntityFrameworkCore;
using POS_API.Interfaces;
using POS_API.Models;

namespace POS_API.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            // .Include() adalah cara EF Core untuk melakukan JOIN
            // agar kita bisa mendapatkan data Kategori dari Produk.
            return await _context.Products
                                 .Include(p => p.Category)
                                 .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(long id)
        {
            return await _context.Products
                                 .Include(p => p.Category)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}