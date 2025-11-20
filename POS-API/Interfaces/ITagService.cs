using POS_API.DTOs;

namespace POS_API.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllAsync();
        Task<TagDto> GetByIdAsync(long id);
        Task<TagDto> CreateAsync(TagCreateDto tagCreateDto);
        Task UpdateAsync(long id, TagUpdateDto tagUpdateDto);
        Task DeleteAsync(long id);
    }
}
