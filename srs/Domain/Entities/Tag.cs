using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Tag : BaseAuditableEntity
    {
        public string Name { get; set; }
        public IEnumerable<Source> Sources { get; set; }
        public IEnumerable<Tag> TagsOf { get; set; }
        public IEnumerable<Tag> ParentTags { get; set; }
        public override bool Equals(object obj)
        {
            var item = obj as Tag;

            if (item == null)
                return false;
            if (Id == item.Id && Name == item.Name && TagsOf == item.TagsOf &&
                ParentTags == item.ParentTags && Sources == item.Sources)
                return true;

            return false;
        }
    }
}
