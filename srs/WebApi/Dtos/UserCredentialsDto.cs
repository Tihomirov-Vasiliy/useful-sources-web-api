using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class UserCredentialsDto
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
