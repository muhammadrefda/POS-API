using POS_API.Models;

namespace POS_API.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<(IEnumerable<Customer> Data, int TotalRecords)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task<Customer> GetByIdAsync(long id);
        Task<Customer> CreateAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(Customer customer);
        Task<Customer> GetByEmailAsync(string email);
    }
}
