using TaskTrack.Service.DTOs;

namespace TaskTrack.Service.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagReadDto>> GetAllAsync();
        Task<TagReadDto> CreateAsync(TagCreateDto dto);
        Task<TagReadDto?> UpdateAsync(int id, TagUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
