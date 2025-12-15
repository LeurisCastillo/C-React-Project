using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CandidatesChannels.Application.Contracts.Security;
using CandidatesChannels.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CandidatesChannels.Infrastructure.Security;

public sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtOptions _options;

    public JwtTokenGenerator(IOptions<JwtOptions> options) => _options = options.Value;

    public string GenerateToken(string email, string role)
    {
        var keyBytes = Encoding.UTF8.GetBytes(_options.Key);
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, email),
            new(JwtRegisteredClaimNames.Email, email),
            new(ClaimTypes.Role, role),
        };

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
