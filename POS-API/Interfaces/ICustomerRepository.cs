using POS_API.Models;

namespace POS_API.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(long id);
        Task<Customer> CreateAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(Customer customer);
        // Tambahan untuk validasi email (Bonus PR)
        Task<Customer> GetByEmailAsync(string email);
    }
}