using Microsoft.EntityFrameworkCore;
using POS_API.Interfaces;
using POS_API.Models;

namespace POS_API.Data.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags.Where(t => t.DeletedAt == null).ToListAsync();
        }

        public async Task<Tag?> GetByIdAsync(long id)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => t.Id == id && t.DeletedAt == null);
        }

        public async Task AddAsync(Tag tag)
        {
           await _context.Tags.AddAsync(tag);
        }

        public void Update(Tag tag) {
            _context.Entry(tag).State = EntityState.Modified;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }


    }
}
