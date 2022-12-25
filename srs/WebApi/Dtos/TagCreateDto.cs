using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos
{
    public class TagCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public IEnumerable<TagOnlyIdDto> ParentTags { get; set; }
    }
}
