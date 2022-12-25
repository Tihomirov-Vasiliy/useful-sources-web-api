using Domain.Entities;

namespace Infrastructure.Data
{
    public interface IUsefulSourcesRepo
    {
        bool SaveChanges();

        IEnumerable<Source> GetAllSourcesWithTreeTags();
        Source GetSourceWithTagsById(int id);
        bool CreateSource(Source source);
        bool UpdateSource(Source source);
        bool DeleteSource(Source source);

        IEnumerable<Tag> GetAllTags();
        Tag GetTagById(int id);
        bool CreateTag(Tag tag);
        bool UpdateTag(Tag tag);
        bool DeleteTag(Tag tag);

        string GetAuthenticatedRole(string userLogin, string userPassword);
    }
}
