using System.Security.Claims;
using IS.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace InventoryShop.Api.Services;

public class TokenService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;


    public TokenService(IConfiguration config)
    {
        _secretKey = config["JWT: Key"] ?? throw new ArgumentNullException("JWT key is missing");
        _issuer = config["JWT: Issuer"] ?? throw new ArgumentNullException("JWT Issuer is missing");
        _audience = config["JWT: Audience"] ?? throw new ArgumentNullException("JWT Audience is missing");
    }

    public string GenerateToken(User user)
    {
        var claims = new Claim[]
        {
            new Claim("id", user.Id.ToString()),
            new Claim("role", user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = creds,
            Issuer = _issuer,
            Audience = _audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);

    }

}
