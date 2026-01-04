using POS_API.Models;
namespace POS_API.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<(IEnumerable<Product> Data, int TotalRecords)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task<Product?> GetByIdAsync(long id);
        Task<Product> CreateAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task<Product> DeleteAsync(Product product);
    }
}
