using Domain.Entities;

namespace Services.ApplicationServices
{
    public interface ITagsService
    {
        IEnumerable<Tag> GetTags(string name = null, int parentTagId = 0, string parentTagName = null);
        Tag GetTagById(int id);
        bool CreateTag(Tag tag);
        bool UpdateTag(Tag tag, Tag patchTag);
        bool DeleteTag(int id);
    }
}
