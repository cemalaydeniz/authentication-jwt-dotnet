using authentication_jwt_dotnet.Models;
using authentication_jwt_dotnet.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace authentication_jwt_dotnet.Services.Implementations
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtToken CreateToken(User user)
        {
            JwtToken token = new JwtToken()
            {
                RefreshToken = GenerateRefreshToken(),
                Expiration = DateTime.Now.AddHours(1)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: token.Expiration,
                notBefore: DateTime.Now,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
                claims: new[]
                {
                    new Claim(ClaimTypes.Name, user.Id)
                } );

            token.AccessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
