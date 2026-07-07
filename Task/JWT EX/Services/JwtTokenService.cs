using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cinema_Management.Models;
using Microsoft.IdentityModel.Tokens;

namespace Cinema_Management.Services;

public sealed class JwtTokenService : IJwtTokenService
{
    private const int DefaultExpiresMinutes = 60;
    private static readonly TimeSpan RememberMeLifetime = TimeSpan.FromDays(30);

    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(User user, bool rememberMe = false)
    {
        var secret = _configuration["JWT_SECRET"];
        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new InvalidOperationException(
                "JWT_SECRET is missing. Set it with User Secrets in Development or an Environment Variable in Production.");
        }

        var issuer = _configuration["Jwt:Issuer"] ?? "CinemaManagement";
        var audience = _configuration["Jwt:Audience"] ?? "CinemaManagementClient";
        var expires = rememberMe
            ? DateTime.UtcNow.Add(RememberMeLifetime)
            : DateTime.UtcNow.AddMinutes(GetExpiresMinutes());

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserID.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private int GetExpiresMinutes()
    {
        var value = _configuration["Jwt:ExpiresMinutes"];
        return int.TryParse(value, out var minutes) && minutes > 0
            ? minutes
            : DefaultExpiresMinutes;
    }
}
