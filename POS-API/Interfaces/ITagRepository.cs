using POS_API.Models;

namespace POS_API.Interfaces
{
    public interface ITagRepository
    {

        Task<IEnumerable<Tag>> GetAllAsync();
        Task<(IEnumerable<Tag> Data, int TotalRecords)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task<Tag?> GetByIdAsync(long id);
        Task AddAsync(Tag tag);
        void Update(Tag tag);
        Task<bool> SaveChangesAsync();
    }
}
