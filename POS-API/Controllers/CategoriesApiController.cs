using Microsoft.AspNetCore.Mvc;
using POS_API.DTOs;
using POS_API.Interfaces;
using POS_API.Helpers; // <-- Pastikan ini ada

namespace POS_API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesApiController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesApiController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();

            // Bungkus dengan ApiResponse
            var response = new ApiResponse<IEnumerable<CategoryDto>>(categories, "Categories retrieved successfully");
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                // Kirim response error 404 menggunakan ApiResponse
                var errorResponse = new ApiResponse<CategoryDto>($"Category with ID {id} not found.");
                return NotFound(errorResponse);
            }

            // Bungkus data dengan ApiResponse
            var response = new ApiResponse<CategoryDto>(category, "Category retrieved successfully");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                // Ambil error dari ModelState
                var errors = ModelState.Values.SelectMany(v => v.Errors).First().ErrorMessage;
                var errorResponse = new ApiResponse<CategoryDto>(errors ?? "Invalid data provided");
                return BadRequest(errorResponse);
            }

            var newCategory = await _categoryService.CreateAsync(categoryDto);

            // Bungkus data dengan ApiResponse
            var response = new ApiResponse<CategoryDto>(newCategory, "Category created successfully");

            // Kembalikan 201 Created
            return CreatedAtAction(nameof(GetById), new { id = newCategory.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] CategoryUpdateDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).First().ErrorMessage;
                var errorResponse = new ApiResponse<object>(errors ?? "Invalid data provided");
                return BadRequest(errorResponse);
            }

            try
            {
                await _categoryService.UpdateAsync(id, categoryDto);

                // Kirim 200 OK dengan body ApiResponse
                var response = new ApiResponse<object>(null, "Category updated successfully");
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                var errorResponse = new ApiResponse<object>(ex.Message);
                return NotFound(errorResponse);
            }
            catch (Exception ex)
            {
                // Penanganan error umum
                var errorResponse = new ApiResponse<object>($"An error occurred: {ex.Message}");
                return StatusCode(500, errorResponse);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);

                // Kirim 200 OK dengan body ApiResponse
                var response = new ApiResponse<object>(null, "Category deleted successfully");
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                var errorResponse = new ApiResponse<object>(ex.Message);
                return NotFound(errorResponse);
            }
            catch (Exception ex)
            {
                // Penanganan error umum
                var errorResponse = new ApiResponse<object>($"An error occurred: {ex.Message}");
                return StatusCode(500, errorResponse);
            }
        }
    }
}