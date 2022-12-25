using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Authentication;
using Domain.Entities;
using Services;

namespace WebApi.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IJwtAuthenticationManager _authenticationService;
        public LoginController(IJwtAuthenticationManager appAuthenticationService)
        {
            _authenticationService = appAuthenticationService;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserCredentialsDto userCredentials)
        {
            var token = _authenticationService.Authenticate(userCredentials.Login,userCredentials.Password);
            if (token != null)
                return Ok(token);

            return NotFound(ErrorMessages.USER_NOT_FOUND);
        }
    }
}
