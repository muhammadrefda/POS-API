using POS_API.DTOs;
using POS_API.Interfaces;
using POS_API.Models;

namespace POS_API.Services
{
    public class CategoryService : ICategoryService
    {
        // Hanya butuh ICategoryRepository
        private readonly ICategoryRepository _categoryRepository;

        // Constructor sudah tidak perlu IMapper
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            // Mapping manual dari List<Category> ke List<CategoryDto>
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.CategoryName
            });
        }

        public async Task<CategoryDto?> GetByIdAsync(long id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return null;
            }

            // Mapping manual dari Category Entity ke CategoryDto
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.CategoryName
            };
        }

        public async Task<CategoryDto> CreateAsync(CategoryCreateDto categoryDto)
        {
            // 1. Mapping dari DTO ke Entity
            var categoryEntity = new Category
            {
                CategoryName = categoryDto.Name
            };

            // 2. Kirim ke Repository untuk disimpan
            var newCategory = await _categoryRepository.CreateAsync(categoryEntity);

            // 3. Mapping kembali ke DTO untuk response
            // (Lebih sederhana dari Product, karena tidak perlu load 
            // navigation property seperti CategoryName)
            return new CategoryDto
            {
                Id = newCategory.Id,
                Name = newCategory.CategoryName
            };
        }

        public async Task UpdateAsync(long id, CategoryUpdateDto categoryDto)
        {
            // 1. Ambil data yang ada dari DB
            var existingCategory = await _categoryRepository.GetByIdAsync(id);

            // 2. Jika tidak ada, throw exception
            if (existingCategory == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }

            // 3. Salin nilai dari DTO ke Entity
            existingCategory.CategoryName = categoryDto.Name;

            // 4. Kirim Entity yang sudah diupdate ke Repository
            await _categoryRepository.UpdateAsync(existingCategory);
        }

        public async Task DeleteAsync(long id)
        {
            var categoryToDelete = await _categoryRepository.GetByIdAsync(id);

            if (categoryToDelete == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }

            await _categoryRepository.DeleteAsync(categoryToDelete);
        }
    }
}