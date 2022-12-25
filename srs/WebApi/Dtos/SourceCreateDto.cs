using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos
{
    public class SourceCreateDto
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [MaxLength(2048)]
        public string Uri { get; set; }

        public IEnumerable <TagOnlyIdDto> Tags { get; set; }
    }
}
