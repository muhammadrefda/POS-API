using Microsoft.EntityFrameworkCore;
using POS_API.Data;
using POS_API.Interfaces;
using POS_API.Models;

namespace POS_API.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            // LOGIKA BARU: Hanya ambil customer yang 'Active' (DeletedAt == null)
            return await _context.Customers
                .Where(c => c.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(long id)
        {
            // LOGIKA BARU: Hanya ambil customer yang 'Active'
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);
        }

        public async Task<Customer> GetByEmailAsync(string email)
        {
            // LOGIKA VALIDASI: Cek email di *semua* record,
            // termasuk yang di-soft-delete, untuk mencegah duplikasi.
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            // BaseEntity menangani CreatedAt
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task UpdateAsync(Customer customer)
        {
            // LOGIKA BARU: Set UpdatedAt secara otomatis
            customer.UpdatedAt = DateTime.Now;
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Customer customer)
        {
            // LOGIKA BARU: Implementasi Soft Delete
            customer.DeletedAt = DateTime.Now;
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}