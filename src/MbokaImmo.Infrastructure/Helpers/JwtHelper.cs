using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MBOKA_IMMO.src.MbokaImmo.Infrastructure.Helpers;

public class JwtHelper
{
    private readonly string _secret;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expiryMinutes;

    public JwtHelper(IConfiguration config)
    {
        _secret = config["Jwt:Secret"]!;
        _issuer = config["Jwt:Issuer"]!;
        _audience = config["Jwt:Audience"]!;
        _expiryMinutes = int.Parse(config["Jwt:ExpiryMinutes"] ?? "60");
    }

    public string GenerateAccessToken(Utilisateur user)
    {
        // Claims de base
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,   user.IdUser.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
            new(ClaimTypes.Role,               user.Role.ToString()),
            new("nom",                         user.Nom),
            new("prenom",                      user.Prenom),
        };

        // Claims spécifiques au rôle
        if (user.Proprietaire is not null)
            claims.Add(new Claim("proprietaireId",
                user.Proprietaire.IdProprio.ToString()));

        if (user.Locataire is not null)
            claims.Add(new Claim("locataireId",
                user.Locataire.IdLocataire.ToString()));

        if (user.Artisan is not null)
            claims.Add(new Claim("artisanId",
                user.Artisan.IdArtisan.ToString()));

        if (user.Agent is not null)
            claims.Add(new Claim("agentId",
                user.Agent.IdAgent.ToString()));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_expiryMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
        => Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");

    public string GenerateEmailVerificationToken(int userId)
    {
        var claims = new[] { new Claim("userId", userId.ToString()) };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public int ValidateEmailVerificationToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _issuer,
            ValidAudience = _audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_secret))
        };

        var principal = handler.ValidateToken(token, parameters, out _);
        return int.Parse(principal.FindFirst("userId")!.Value);
    }
}
