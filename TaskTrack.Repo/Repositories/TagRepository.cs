using Microsoft.EntityFrameworkCore;
using TaskTrack.Repo.Data;
using TaskTrack.Repo.Models;

namespace TaskTrack.Repo.Repositories
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(TaskTrackDbContext context) : base(context) { }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<bool> IsUsedByTaskAsync(int tagId)
        {
            return await _context.TaskTags.AnyAsync(tt => tt.TagID == tagId);
        }
    }
}
