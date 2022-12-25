using System.Collections.Generic;
using System.Linq;
using Infrastructure.Data;
using Services.ApplicationServices;
using Domain.Exceptions;
using Domain.Entities;
using Xunit;

namespace UsefulSourcesServices.Tests.ApplicationServiceUnitTests
{
    public class SourceServiceUnitTests
    {
        private IUsefulSourcesRepo _mockRepo;
        private ISourcesService _sourcesService;
        [Fact]
        public void GetSources_NothingInDatabase_ThrowsObjectNotFoundException()
        {
            _mockRepo = new MockUsefulSourcesRepo(null, null);
            _sourcesService = new SourcesService(_mockRepo);

            Assert.Throws<ObjectNotFoundException>(() => _sourcesService.GetSources());
        }
        [Fact]
        public void GetSources_TwoSourcesInDatabase_ReturnsTwo()
        {
            List<Source> mockSources = new List<Source>()
            {
                new Source()
                {
                    Id=1,
                    Name="1 Source",
                    Uri="1sourceuri"
                },
                new Source()
                {
                    Id=2,
                    Name="3 Source",
                    Uri="2sourceuri"
                }
            };
            _mockRepo = new MockUsefulSourcesRepo(null, mockSources);
            _sourcesService = new SourcesService(_mockRepo);

            List<Source> souresFromAppService = _sourcesService.GetSources().ToList();
            Assert.Equal(souresFromAppService, mockSources);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(100)]
        [InlineData(10000)]
        [InlineData(null)]
        public void GetSourceById_NoSources_ThrowsObjectNotFoundException(int id)
        {
            _mockRepo = new MockUsefulSourcesRepo(null, null);
            _sourcesService = new SourcesService(_mockRepo);

            Assert.Throws<ObjectNotFoundException>(() => _sourcesService.GetSourceById(id));
        }
        [Theory]
        [InlineData(10)]
        [InlineData(1000)]
        [InlineData(100)]
        [InlineData(1500)]
        [InlineData(null)]
        public void GetSourceById_NoSourcesWithThatId_ThrowsObjectNotFoundException(int id)
        {
            var mockSources = new List<Source>()
            {
                new Source(){Id=1,Name="1"},
                new Source(){Id=2,Name="2"}
            };
            _mockRepo = new MockUsefulSourcesRepo(null, mockSources);
            _sourcesService = new SourcesService(_mockRepo);

            Assert.Throws<ObjectNotFoundException>(() => _sourcesService.GetSourceById(id));
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetSourceById_ThereAreSourcesWithThatId_ReturnsSource(int id)
        {
            var mockSources = new List<Source>() { new Source() { Id = 1 }, new Source() { Id = 2 } };

            _mockRepo = new MockUsefulSourcesRepo(null, mockSources);
            _sourcesService = new SourcesService(_mockRepo);

            Assert.Equal(mockSources[id - 1], _sourcesService.GetSourceById(id));
        }
        [Fact]
        public void CreateSource_ThereAreNoTagsInDatabase_ThrowWrongInputDataException()
        {
            var mockTags = new List<Tag>() { new Tag { Id = 1, Name = "Tagname" } };
            Source newSource = new Source() { Id = 1, Tags = new List<Tag>() { new Tag() { Id = 2, Name = "TagNotInDb" } } };
            _mockRepo = new MockUsefulSourcesRepo(mockTags, null);
            _sourcesService = new SourcesService(_mockRepo);

            Assert.Throws<WrongInputDataException>(() => _sourcesService.CreateSource(newSource));
        }
        [Fact]
        public void CreateSource_NullInsteadSource_ThrowObjectNotFoundException()
        {
            var mockTags = new List<Tag>() { new Tag { Id = 1, Name = "Tagname" } };
            Source newSource = null;
            _mockRepo = new MockUsefulSourcesRepo(mockTags, null);
            _sourcesService = new SourcesService(_mockRepo);

            Assert.Throws<ObjectNotFoundException>(() => _sourcesService.CreateSource(newSource));
        }
        [Fact]
        public void CreateSource_RigthSource_ReturnsTrue()
        {
            Source newSource = new Source() { Id = 1, Name = "Source" };
            _mockRepo = new MockUsefulSourcesRepo(null, null);
            _sourcesService = new SourcesService(_mockRepo);
            Assert.True(_sourcesService.CreateSource(newSource));
        }
        [Fact]
        public void UpdateSource_SourceWithTagsThatDontExistInDatabase_ThrowsWrongInputDataException()
        {
            Tag tagInDb = new Tag()
            {
                Id = 1,
                Name = "FirstTag",
                Sources = null,
                ParentTags = null
            };
            Tag tagNotInDb = new Tag()
            {
                Id = 2,
                Name = "SecondTag",
                Sources = null,
                ParentTags = null
            };
            List<Tag> mockTags = new List<Tag>();
            List<Tag> tagsForPatch = new List<Tag>();
            tagsForPatch.Add(tagNotInDb);
            mockTags.Add(tagInDb);
            Source source = new Source()
            {
                Id = 1,
                Name = "SourceModel",
                Tags = mockTags
            };
            Source patchSource = new Source()
            {
                Id = 1,
                Name = "SourcePatchModel",
                Tags = tagsForPatch
            };

            _mockRepo = new MockUsefulSourcesRepo(mockTags, null);
            _sourcesService = new SourcesService(_mockRepo);

            Assert.Throws<WrongInputDataException>(() => _sourcesService.UpdateSource(source, patchSource));
        }
        [Fact]
        public void UpdateSource_RigthInput_ReturnsTrue()
        {
            Tag tagInDb = new Tag()
            {
                Id = 1,
                Name = "FirstTag",
                Sources = null,
                ParentTags = null
            };
            List<Tag> mockTags = new List<Tag>();
            List<Tag> tagsForPatch = new List<Tag>();
            tagsForPatch.Add(tagInDb);
            mockTags.Add(tagInDb);
            Source source = new Source()
            {
                Id = 1,
                Name = "SourceModel",
                Tags = mockTags
            };
            Source patchSource = new Source()
            {
                Id = 1,
                Name = "SourcePatchModel",
                Tags = tagsForPatch
            };

            _mockRepo = new MockUsefulSourcesRepo(mockTags, null);
            _sourcesService = new SourcesService(_mockRepo);

            Assert.True(_sourcesService.UpdateSource(source, patchSource));
        }
        [Theory]
        [InlineData(null)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(1000)]
        public void DeleteSource_NoSourceInDatabase_ThrowsObjectNotFoundException(int id)
        {
            _mockRepo = new MockUsefulSourcesRepo(null, null);
            _sourcesService = new SourcesService(_mockRepo);
            Assert.Throws<ObjectNotFoundException>(() => _sourcesService.DeleteSource(id));
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void DeleteSource_ThereAreSourcesWithThatTag_ReturnsTrue(int id)
        {
            var soures = new List<Source>() { new Source() { Id = 1 }, new Source() { Id = 2 }, new Source() { Id = 3 } };
            _mockRepo = new MockUsefulSourcesRepo(null, soures);
            _sourcesService = new SourcesService(_mockRepo);
            Assert.True(_sourcesService.DeleteSource(id));
        }
    }
}