using Microsoft.EntityFrameworkCore;
using POS_API.Data;
using POS_API.Interfaces;
using POS_API.Models;

namespace POS_API.Data.Repositories
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
            return await _context.Customers.Where(c => c.DeletedAt == null).ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(long id)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            try
            {

                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();
                return customer;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task UpdateAsync(Customer customer)
        {
            customer.UpdatedAt = DateTime.Now;
             _context.Entry(customer).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Customer customer)
        {
            customer.DeletedAt = DateTime.Now;
            _context.Entry(customer).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }


        public async Task<Customer> GetByEmailAsync(string email)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
        }
    }
}
