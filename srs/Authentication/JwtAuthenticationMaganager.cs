using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Infrastructure.Data;

namespace Authentication
{
    public class JwtAuthenticationMaganager : IJwtAuthenticationManager
    {
        private readonly IUsefulSourcesRepo _repo;
        private readonly IConfiguration _config;

        public JwtAuthenticationMaganager(IConfiguration config, IUsefulSourcesRepo repo)
        {
            _config = config;
            _repo = repo;
        }
        public string Authenticate(string login, string password)
        {
            //check if user authenticated and get role
            string userRole = _repo.GetAuthenticatedRole(login, password);
            if (userRole == null)
                return null;

            //creating JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, login),
                    new Claim(ClaimTypes.Role, userRole)
                }),
                Expires = DateTime.Now.AddMinutes(15),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),
                Audience = _config["Jwt:Audience"],
                Issuer = _config ["Jwt:Issuer"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }       
    }
}