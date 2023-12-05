using authentication_jwt_dotnet.Models;

namespace authentication_jwt_dotnet.Services.Interfaces
{
    public interface IJwtTokenService
    {
        JwtToken CreateToken(User user);
    }
}
