using POS_API.Helpers;
using POS_API.DTOs;
using POS_API.Models;

namespace POS_API.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<PagedResponse<ProductDto>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task<ProductDto?> GetByIdAsync(long id);
        Task<ProductDto> CreateAsync(ProductCreateDto productDto);
        Task UpdateAsync(long id, ProductUpdateDto productDto);
        Task DeleteAsync(long id);
    }
}
