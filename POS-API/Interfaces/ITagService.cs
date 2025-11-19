using POS_API.DTOs;

namespace POS_API.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllAsync();
        Task<TagDto> GetByIdAsync(long id);
        Task<TagDto> CreateAsync(TagCreateDto tagDto);
        Task UpdateAsync(long id, TagUpdateDto tagDto);
        Task DeleteAsync(long id);
    }
}