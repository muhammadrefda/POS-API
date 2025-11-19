using POS_API.DTOs;
using POS_API.Interfaces;
using POS_API.Models;

namespace POS_API.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IEnumerable<TagDto>> GetAllAsync()
        {
            var tags = await _tagRepository.GetAllAsync();
            return tags.Select(tag => new TagDto
            {
                Id = tag.Id,
                TagName = tag.TagName
            });
        }

        public async Task<TagDto> GetByIdAsync(long id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null) return null;

            return new TagDto
            {
                Id = tag.Id,
                TagName = tag.TagName
            };
        }

        public async Task<TagDto> CreateAsync(TagCreateDto tagDto)
        {
            var tag = new Tag
            {
                TagName = tagDto.TagName,
                CreatedAt = DateTime.Now
            };

            await _tagRepository.AddAsync(tag);
            await _tagRepository.SaveChangesAsync();

            return new TagDto
            {
                Id = tag.Id,
                TagName = tag.TagName
            };
        }

        public async Task UpdateAsync(long id, TagUpdateDto tagDto)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {id} not found");
            }

            tag.TagName = tagDto.TagName;
            tag.UpdatedAt = DateTime.Now;

            _tagRepository.Update(tag);
            await _tagRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {id} not found");
            }

            // Soft Delete
            tag.DeletedAt = DateTime.Now;
            _tagRepository.Update(tag);
            await _tagRepository.SaveChangesAsync();
        }
    }
}