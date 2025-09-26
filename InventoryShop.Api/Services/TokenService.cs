namespace IS.Services;
using IS.Entities;
using IS.Enums;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;


public class TokenService
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;

    public TokenService(IConfiguration config)
    {
        _key = config["Jwt: Key"] ?? throw new ArgumentNullException("JWT: Key is missing");
        _issuer = config["Jwt: Issuer"] ?? throw new ArgumentNullException("JWT: Issuer is missing");
        _audience = config["Jwt: Audience"] ?? throw new ArgumentNullException("JWT: Audience is missing");
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim("id", user.Id.ToString()),
            new Claim("role", user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = creds,
            Issuer = _issuer,
            Audience = _audience
        };

        var tokenHanlder = new JwtSecurityTokenHandler();
        var token = tokenHanlder.CreateToken(tokenDescriptor);

        return tokenHanlder.WriteToken(token);


    }




}