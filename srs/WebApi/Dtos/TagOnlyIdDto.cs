using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos
{
    public class TagOnlyIdDto
    {
        [Required]
        public int Id { get; set; }
    }
}
