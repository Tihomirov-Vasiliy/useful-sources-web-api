using Domain.Common;

namespace Domain.Entities
{
    public class User : BaseAuditableEntity
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string EmailAddress { get; set; }
        public string GivenName { get; set; }
    }
}
