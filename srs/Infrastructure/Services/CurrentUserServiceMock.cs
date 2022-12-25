using Application.Common.Interfaces;

namespace Infrastructure.Services
{
    public class CurrentUserServiceMock : ICurrentUserService
    {
        public string UserId => "User";
    }
}
