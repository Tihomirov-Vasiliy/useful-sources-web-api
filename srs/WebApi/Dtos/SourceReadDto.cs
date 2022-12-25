namespace WebApi.Dtos
{
    public class SourceReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public IEnumerable<TagReadDto> Tags { get; set; }
    }
}
