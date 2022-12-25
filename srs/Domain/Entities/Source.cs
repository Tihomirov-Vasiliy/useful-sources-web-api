using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Source : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string Uri { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public override bool Equals(object obj)
        {
            Source source = obj as Source;
            if (source == null)
                return false;
            if (Id == source.Id && Name == source.Name && Uri == source.Uri && Tags == source.Tags)
                return true;
            return false;
        }
    }
}
