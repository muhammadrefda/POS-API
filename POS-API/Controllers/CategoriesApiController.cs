using Microsoft.AspNetCore.Mvc;
using POS_API.Data;
using POS_API.Models;
using Microsoft.EntityFrameworkCore; 

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // 2. Buat Constructor untuk menerima DbContext (Dependency Injection)
        public CategoriesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CategoriesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            // 3. Ambil data dari database, bukan static list
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }

        // GET: api/CategoriesApi/5 (Method tambahan, PENTING untuk POST)
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // POST: api/CategoriesApi
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory([FromBody] Category category)
        {
            if (category == null || string.IsNullOrEmpty(category.CategoryName))
            {
                return BadRequest("Nama kategori tidak boleh kosong.");
            }

            // 4. Tambahkan data ke DbContext
            _context.Categories.Add(category);

            // 5. Simpan perubahan ke database
            await _context.SaveChangesAsync();

            // Return 201 Created dengan data yang baru dibuat
            // Menggunakan method GetCategory yang baru kita buat
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        // DELETE: api/CategoriesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            // 6. Cari data di database
            var categoryToDelete = await _context.Categories.FindAsync(id);

            if (categoryToDelete == null)
            {
                return NotFound();
            }

            // 7. Hapus data dari DbContext
            _context.Categories.Remove(categoryToDelete);

            // 8. Simpan perubahan ke database
            await _context.SaveChangesAsync();

            return NoContent(); // Sukses
        }
    }
}
