using POS_API.Helpers;
using POS_API.DTOs;
using POS_API.Models;
namespace POS_API.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllAsync();
        Task<PagedResponse<CustomerDto>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task<CustomerDto?> GetByIdAsync(long id);
        Task<CustomerDto> CreateAsync(CustomerCreateDto customerDto);
        Task UpdateAsync(long id, CustomerUpdateDto customerDto);
        Task DeleteAsync(long id);
    }
}
