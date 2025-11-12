using POS_API.DTOs;

namespace POS_API.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetByIdAsync(long id);
        Task<CategoryDto> CreateAsync(CategoryCreateDto categoryDto);
        Task UpdateAsync(long id, CategoryUpdateDto categoryDto);
        Task DeleteAsync(long id);
    }
}