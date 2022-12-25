using Domain.Entities;

namespace Infrastructure.Data
{
    public class MockUsefulSourcesRepo : IUsefulSourcesRepo
    {
        private List<Tag> _tags;
        private List<Source> _sources;
        public MockUsefulSourcesRepo()
        {
            List<Tag> mockTags = new List<Tag>()
            {
                new Tag()
                {
                    Id=1,
                    Name="FirstTag",
                    Sources = null,
                    ParentTags = null
                },
                new Tag()
                {
                    Id=2,
                    Name  = "Second tag",
                    Sources = null,
                    ParentTags = null
                }
            };
            List<Source> mockSources = new List<Source>()
            {
                new Source(){Id=1,Name="Source1",Uri="Uri1",Tags = new List<Tag>(){ mockTags[0] } },
                new Source(){Id=1,Name="Source2",Uri="Uri2",Tags = new List<Tag>(){ mockTags[1] } }
            };
            _tags = mockTags;
            _sources = mockSources;
        }
        public MockUsefulSourcesRepo(List<Tag> mockTags, List<Source> mockSources)
        {
            if (mockTags == null)
                _tags = new List<Tag>();
            else
                _tags = mockTags;
            if (mockSources==null)
                _sources = new List<Source>();
            else
                _sources = mockSources;
        }
        public bool CreateSource(Source source)
        {
            _sources.Add(source);
            return true;
        }
        public bool CreateTag(Tag tag)
        {
            _tags.Add(tag);
            return true;
        }
        public bool DeleteSource(Source source)
        {
            return true;
        }
        public bool DeleteTag(Tag tag)
        {
            return true;
        }
        public IEnumerable<Source> GetAllSourcesWithTreeTags()
        {
            return _sources;
        }
        public IEnumerable<Tag> GetAllTags()
        {
            return _tags;
        }
        public string GetAuthenticatedRole(string userLogin, string userPassword)
        {
            throw new NotImplementedException();
        }
        public Source GetSourceWithTagsById(int id)
        {
            if (_sources != null)
                return _sources.FirstOrDefault(source => source.Id == id);
            return null;
        }
        public Tag GetTagById(int id)
        {
            if (_tags != null)
                if (_tags.FirstOrDefault(tag => tag.Id == id) != null)
                    return _tags.FirstOrDefault(tag => tag.Id == id);
            return null;
        }
        public bool SaveChanges()
        {
            return true;
        }
        public bool UpdateSource(Source source)
        {
            return true;
        }
        public bool UpdateTag(Tag tag)
        {
            return true;
        }
    }
}
