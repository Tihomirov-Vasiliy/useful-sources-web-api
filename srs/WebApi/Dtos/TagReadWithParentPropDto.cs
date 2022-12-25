namespace WebApi.Dtos
{
    public class TagReadWithParentPropDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<TagReadDto> ParentTags { get; set; }
    }
}
