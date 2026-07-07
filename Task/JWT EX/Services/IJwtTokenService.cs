using Cinema_Management.Models;

namespace Cinema_Management.Services;

public interface IJwtTokenService
{
    string CreateToken(User user, bool rememberMe = false);
}
