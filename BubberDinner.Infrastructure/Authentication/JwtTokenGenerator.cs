using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BubberDinner.Application.Common.Interfaces.Authentication;
using Microsoft.IdentityModel.Tokens;
using BubberDinner.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Options;
using BubberDinner.Domain.Entities;

namespace BubberDinner.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _jwtSettings;
    public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtOptions)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateToken(User user)
    {
        var signingCredentials = CreateSigningCredentials();

        var claims = new []
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: signingCredentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return token;
    }

    #region Local

    private SigningCredentials CreateSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
        var symmetricSecurityKey = new SymmetricSecurityKey(key);
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        return signingCredentials;
    }

    #endregion
}