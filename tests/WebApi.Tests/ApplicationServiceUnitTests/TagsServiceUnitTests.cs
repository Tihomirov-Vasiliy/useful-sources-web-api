using System.Collections.Generic;
using System.Linq;
using Infrastructure.Data;
using Services.ApplicationServices;
using Domain.Exceptions;
using Domain.Entities;
using Xunit;

namespace UsefulSourcesServices.Tests.ApplicationServiceUnitTests
{
    public class TagServiceUnitTests
    {
        private IUsefulSourcesRepo _mockRepo;
        private ITagsService _tagsService;
        [Fact]
        public void GetTags_NothingInDatabase_ThrowsObjectNotFoundException()
        {
            _mockRepo = new MockUsefulSourcesRepo(null, null);
            _tagsService = new TagsService(_mockRepo);

            Assert.Throws<ObjectNotFoundException>(() => _tagsService.GetTags());
        }
        [Fact]
        public void GetTags_TwoTagsInDatabase_ReturnsTwoTags()
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
            _mockRepo = new MockUsefulSourcesRepo(mockTags, null);
            _tagsService = new TagsService(_mockRepo);

            List<Tag> tagsFromAppService = _tagsService.GetTags().ToList();

            Assert.Equal(tagsFromAppService, mockTags);
        }
        [Fact]
        public void GetTagById_NoTags_ThrowsObjectNotFoundException()
        {
            _mockRepo = new MockUsefulSourcesRepo(null, null);
            _tagsService = new TagsService(_mockRepo);

            Assert.Throws<ObjectNotFoundException>(() => _tagsService.GetTagById(1));
        }
        [Theory]
        [InlineData(null)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(1000)]
        public void GetTagById_NoTagWithThatId_ThrowsObjectNotFoundException(int id)
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
            _mockRepo = new MockUsefulSourcesRepo(mockTags, null);
            _tagsService = new TagsService(_mockRepo);

            Assert.Throws<ObjectNotFoundException>(() => _tagsService.GetTagById(id));
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetTagById_TagWithThatIdExists_GetThatTag(int id)
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
            _mockRepo = new MockUsefulSourcesRepo(mockTags, null);
            _tagsService = new TagsService(_mockRepo);

            Assert.Equal(mockTags[id - 1], _tagsService.GetTagById(id));
        }
        [Fact]
        public void CreateTag_NullInsteadTag_ThrowsObjectNotFoundException()
        {
            _mockRepo = new MockUsefulSourcesRepo(null, null);
            _tagsService = new TagsService(_mockRepo);
            Tag tag = null;

            Assert.Throws<ObjectNotFoundException>(() => _tagsService.CreateTag(tag));
        }
        [Fact]
        public void CreateTag_WrongTagInput_ThrowsWrongInputDataException()
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
            _mockRepo = new MockUsefulSourcesRepo(mockTags, null);
            _tagsService = new TagsService(_mockRepo);

            Tag tag = new Tag()
            {
                Id = 3,
                Name = "wrongTag",
                ParentTags = new List<Tag>() { new Tag() { Id = 4 } }
            };

            Assert.Throws<WrongInputDataException>(() => _tagsService.CreateTag(tag));
        }
        [Fact]
        public void CreateTag_RigthTagInpug_ReturnsNTrue()
        {
            List<Tag> mockTags = new List<Tag>()
            {
                new Tag()
                {
                    Id=1,
                    Name="FirstTag",
                    Sources = null,
                    ParentTags = null
                }
            };
            _mockRepo = new MockUsefulSourcesRepo(mockTags, null);
            _tagsService = new TagsService(_mockRepo);

            Tag tag = new Tag()
            {
                Id = 2,
                Name = "Second tag",
                Sources = null,
                ParentTags = null
            };

            Assert.True(_tagsService.CreateTag(tag));
        }
        [Fact]
        public void UpdateTag_WrongTagInput_ThrowsWrongInputDataException()
        {
            Tag tagInDb = new Tag()
            {
                Id = 1,
                Name = "FirstTag",
                Sources = null,
                ParentTags = null
            };
            List<Tag> mockTags = new List<Tag>();
            mockTags.Add(tagInDb);
            _mockRepo = new MockUsefulSourcesRepo(mockTags, null);
            _tagsService = new TagsService(_mockRepo);
            List<Tag> parentTags = new List<Tag>();
            parentTags.Add(tagInDb);
            Tag tag = new Tag()
            {
                Id = 3,
                Name = "Tag 3",
                ParentTags = parentTags
            };


            Tag tagPatch = new Tag()
            {
                Id = 3,
                Name = "Tag 3 Patched",
                ParentTags = new List<Tag>() { new Tag() { Id = 4 } }
            };

            Assert.Throws<WrongInputDataException>(() => _tagsService.UpdateTag(tag, tagPatch));
        }
        [Fact]
        public void UpdateTag_RightagInput_ReturnsTrue()
        {
            Tag tagInDb = new Tag()
            {
                Id = 1,
                Name = "FirstTag",
                Sources = null,
                ParentTags = null
            };
            List<Tag> mockTags = new List<Tag>();
            mockTags.Add(tagInDb);
            _mockRepo = new MockUsefulSourcesRepo(mockTags, null);
            _tagsService = new TagsService(_mockRepo);

            List<Tag> parentTags = new List<Tag>();
            parentTags.Add(tagInDb);
            Tag tag = new Tag()
            {
                Id = 3,
                Name = "Tag 3",
                ParentTags = parentTags
            };


            Tag tagPatch = new Tag()
            {
                Id = 3,
                Name = "Tag 3 Patched",
                ParentTags = parentTags
            };
            Assert.True(_tagsService.UpdateTag(tag, tagPatch));
        }
        [Theory]
        [InlineData(null)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(1000)]
        public void DeleteTag_NoTagsWithThatId_ThrowsObjectNotFoundException(int id)
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
            _mockRepo = new MockUsefulSourcesRepo(mockTags, null);
            _tagsService = new TagsService(_mockRepo);

            Assert.Throws<ObjectNotFoundException>(() => _tagsService.DeleteTag(id));
        }
        [Fact]
        public void DeleteTag_TagHasChildTags_ThrowsWrongInputDataException()
        {
            List<Tag> mockTags = new List<Tag>()
            {
                new Tag()
                {
                    Id=1,
                    Name="FirstTag",
                    Sources = null,
                    ParentTags = null,
                    TagsOf = new List<Tag>(){new Tag() { Id=3}, new Tag() { Id=4} }
                },
                new Tag()
                {
                    Id=2,
                    Name  = "Second tag",
                    Sources = null,
                    ParentTags = null,
                    TagsOf = new List<Tag>(){new Tag() { Id=3}, new Tag() { Id=4} }
                }
            };

            _mockRepo = new MockUsefulSourcesRepo(mockTags, null);
            _tagsService = new TagsService(_mockRepo);

            Assert.Throws<WrongInputDataException>(() => _tagsService.DeleteTag(1));
        }
        [Fact]
        public void DeleteTag_RigthInput_ReturnsTrue()
        {
            List<Tag> mockTags = new List<Tag>()
            {
                new Tag()
                {
                    Id=1,
                    Name="FirstTag",
                    Sources = null,
                    ParentTags = null,
                    TagsOf =null
                }
            };
            _mockRepo = new MockUsefulSourcesRepo(mockTags, null);
            _tagsService = new TagsService(_mockRepo);
            Assert.True(_tagsService.DeleteTag(1));
        }
    }
}