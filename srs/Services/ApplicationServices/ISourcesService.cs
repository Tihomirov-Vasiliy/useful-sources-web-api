using Domain.Entities;

namespace Services.ApplicationServices
{
    public interface ISourcesService
    {
        IEnumerable<Source> GetSources(string name = null, string uri = null, int tagId = 0, string tagName = null);
        Source GetSourceById(int id);
        bool CreateSource(Source source);
        bool UpdateSource(Source sourceModel, Source patchSource);
        bool DeleteSource(int id);
    }
}
