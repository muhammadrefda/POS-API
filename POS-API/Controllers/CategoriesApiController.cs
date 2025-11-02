using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS_API.Data;
using POS_API.Models;
using POS_API.Helpers; // <-- 1. JANGAN LUPA using helper ApiResponse

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CategoriesApi
        // Perubahan 1: Return type diubah ke ApiResponse
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Category>>>> GetCategories()
        {
            try // Perubahan 2: Dibungkus try-catch
            {
                var categories = await _context.Categories.ToListAsync();

                // Perubahan 3: Dibungkus ApiResponse sukses
                return Ok(new ApiResponse<IEnumerable<Category>>(categories, "Data kategori berhasil diambil"));
            }
            catch (Exception ex)
            {
                // Perubahan 4: Dibungkus ApiResponse gagal
                return StatusCode(500, new ApiResponse<IEnumerable<Category>>($"Error server: {ex.Message}"));
            }
        }

        // GET: api/CategoriesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Category>>> GetCategory(long id) // Standarisasi ke long
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);

                if (category == null)
                {
                    return NotFound(new ApiResponse<Category>("Kategori tidak ditemukan."));
                }

                return Ok(new ApiResponse<Category>(category, "Kategori ditemukan."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<Category>($"Error server: {ex.Message}"));
            }
        }

        // POST: api/CategoriesApi
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Category>>> PostCategory([FromBody] Category category)
        {
            if (category == null || string.IsNullOrEmpty(category.CategoryName))
            {
                return BadRequest(new ApiResponse<Category>("Nama kategori tidak boleh kosong."));
            }

            if (await _context.Categories.AnyAsync(c => c.CategoryName == category.CategoryName))
            {
                return BadRequest(new ApiResponse<Category>("Kategori dengan nama tersebut sudah ada."));
            }

            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                var apiResponse = new ApiResponse<Category>(category, "Kategori berhasil ditambahkan.");
                return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, apiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<Category>($"Error saat menyimpan: {ex.Message}"));
            }
        }

        // PUT: api/CategoriesApi/5
        // Diubah ke PUT agar konsisten dengan ProductsApi dan logika modal
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> PutCategory(long id, [FromBody] Category category)
        {
            if (id != category.Id)
            {
                return BadRequest(new ApiResponse<object>("ID kategori tidak cocok."));
            }

            if (await _context.Categories.AnyAsync(c => c.CategoryName == category.CategoryName && c.Id != id))
            {
                return BadRequest(new ApiResponse<object>("Nama kategori tersebut sudah digunakan."));
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(new ApiResponse<object>("Kategori tidak ditemukan (kemungkinan sudah dihapus)."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>($"Error saat update: {ex.Message}"));
            }

            return Ok(new ApiResponse<object>(null, "Kategori berhasil diperbarui."));
        }

        // DELETE: api/CategoriesApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteCategory(long id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound(new ApiResponse<object>("Kategori tidak ditemukan."));
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<object>(null, "Kategori berhasil dihapus."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>($"Error saat menghapus: {ex.Message}"));
            }
        }
    }
}