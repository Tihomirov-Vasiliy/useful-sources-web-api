using Domain.Entities;

namespace Services
{
    public static class Convertor
    {
        public static IEnumerable<Source> ParentTagTreeToLinear(IEnumerable<Source> sources)
        {
            foreach (var source in sources)
            {
                source.Tags = ConvertTagsFromTreeToLinearDistinct(source.Tags);
            }
            return sources;
        }
        public static Source ParentTagTreeToLinear(Source source)
        {
            source.Tags = ConvertTagsFromTreeToLinearDistinct(source.Tags);
            return source;
        }
        public static IEnumerable<Tag> TagsToTagsWithFirstParents(IEnumerable<Tag> tags)
        {
            var newTagList = new List<Tag>();

            foreach (Tag tag in tags)
            {
                newTagList.Add(TagToTagWithFirstParents(tag));
            }
            return newTagList;
        }
        public static Tag TagToTagWithFirstParents(Tag tag)
        {
            Tag currentTag;
            currentTag = new Tag
            {
                Id = tag.Id,
                Name = tag.Name,
                ParentTags = tag.ParentTags,
                TagsOf = tag.TagsOf,
                Sources = null
            };
            if (currentTag.ParentTags != null)
                foreach (Tag parentTag in currentTag.ParentTags)
                {
                    parentTag.ParentTags = null;
                    parentTag.TagsOf = null;
                    parentTag.Sources = null;
                }

            return currentTag;
        }

        //Подумать о том как оптимизировать алгоритм т.к. он рекурсивный и требует затрат(а вообще нужно ли его менять?)
        private static IEnumerable<Tag> ConvertTagsFromTreeToLinearDistinct(IEnumerable<Tag> tags)
        {
            Tag currentTag;
            bool isContinueConverting = false;
            ICollection<Tag> newTags = new List<Tag>();

            foreach (var tag in tags)
            {
                currentTag = new Tag
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    ParentTags = tag.ParentTags
                };
                if (currentTag.ParentTags != null)
                {
                    isContinueConverting = true;
                    foreach (var parentTag in currentTag.ParentTags)
                    {
                        newTags.Add(parentTag);
                    }
                    currentTag.ParentTags = null;
                }
                newTags.Add(currentTag);
            }

            if (isContinueConverting == false)
                return newTags.DistinctBy(t => t.Id).OrderBy(t => t.Id);
            else
                return ConvertTagsFromTreeToLinearDistinct(newTags);
        }
    }
}
