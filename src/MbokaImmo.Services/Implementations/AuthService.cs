
using MBOKA_IMMO.src.MbokaImmo.API.DTOs.Auth;
using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;
using MBOKA_IMMO.src.MbokaImmo.Infrastructure.Helpers;
using MBOKA_IMMO.src.MbokaImmo.Infrastructure.Persistence;
using MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBOKA_IMMO.src.MbokaImmo.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly JwtHelper _jwtHelper;
    private const int RefreshTokenJours = 7;

    public AuthService(AppDbContext context, JwtHelper jwtHelper)
    {
        _context = context;
        _jwtHelper = jwtHelper;
    }

    private async Task<string> CreerRefreshTokenAsync(int idUser)
    {
        var refreshToken = new RefreshToken
        {
            Token = _jwtHelper.GenerateRefreshToken(),
            IdUser = idUser,
            DateExpiration = DateTime.UtcNow.AddDays(RefreshTokenJours),
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return refreshToken.Token;
    }

    public async Task<(AuthResponseDto Response, string RefreshToken)> RegisterAsync(RegisterRequestDto dto)
    {
        var emailExiste = await _context.Utilisateurs
            .AnyAsync(u => u.Email == dto.Email);

        if (emailExiste)
            throw new InvalidOperationException("Cet email est déjà utilisé.");

        if (!Enum.TryParse<RoleEnum>(dto.Role, true, out var role))
            throw new ArgumentException(
                "Rôle invalide. Valeurs acceptées : Proprio, Locataire, Agent, Artisan.");

        var utilisateur = new Utilisateur
        {
            Nom = dto.Nom,
            Prenom = dto.Prenom,
            Email = dto.Email,
            MotDePasse = PasswordHelper.Hash(dto.MotDePasse),
            Telephone = dto.Telephone,
            PaysResidence = dto.PaysResidence,
            VilleResidence = dto.VilleResidence,
            Role = role,
            DateInscription = DateTime.UtcNow,
            CompteActif = true,
            KycValide = false,
        };

        _context.Utilisateurs.Add(utilisateur);

        switch (role)
        {
            case RoleEnum.Proprio:
                _context.Proprietaires.Add(new Proprietaire
                {
                    Utilisateur = utilisateur
                });
                break;

            case RoleEnum.Locataire:
                _context.Locataires.Add(new Locataire
                {
                    Utilisateur = utilisateur
                });
                break;

            case RoleEnum.Agent:
                _context.Agents.Add(new Agent
                {
                    Utilisateur = utilisateur
                });
                break;

            case RoleEnum.Artisan:
                _context.Artisans.Add(new Artisan
                {
                    Utilisateur = utilisateur,
                    Specialite = "Non définie"
                });
                break;
        }

        await _context.SaveChangesAsync();

        var utilisateurComplet = await _context.Utilisateurs
            .Include(u => u.Proprietaire)
            .Include(u => u.Locataire)
            .Include(u => u.Artisan)
            .Include(u => u.Agent)
            .FirstAsync(u => u.IdUser == utilisateur.IdUser);

        var refreshToken = await CreerRefreshTokenAsync(utilisateurComplet.IdUser);
        var response = new AuthResponseDto
        {
            AccessToken = _jwtHelper.GenerateAccessToken(utilisateurComplet),
            User = MapToUserDto(utilisateurComplet)
        };

        return (response, refreshToken);
    }

    public async Task<(AuthResponseDto Response, string RefreshToken)> LoginAsync(LoginRequestDto dto)
    {
       
        var utilisateur = await _context.Utilisateurs
            .Include(u => u.Proprietaire)
            .Include(u => u.Locataire)
            .Include(u => u.Artisan)
            .Include(u => u.Agent)
            .FirstOrDefaultAsync(u => u.Email == dto.Email && u.CompteActif);

        if (utilisateur is null)
            throw new UnauthorizedAccessException("Email ou mot de passe incorrect.");

        if (!PasswordHelper.Verify(dto.MotDePasse, utilisateur.MotDePasse))
            throw new UnauthorizedAccessException("Email ou mot de passe incorrect.");

        var refreshToken = await CreerRefreshTokenAsync(utilisateur.IdUser);
        var response = new AuthResponseDto
        {
            AccessToken = _jwtHelper.GenerateAccessToken(utilisateur),
            User = MapToUserDto(utilisateur)
        };

        return (response, refreshToken);
    }

    public async Task<(AuthResponseDto Response, string RefreshToken)> RefreshAsync(string refreshToken)
    {
        var token = await _context.RefreshTokens
            .Include(r => r.Utilisateur).ThenInclude(u => u.Proprietaire)
            .Include(r => r.Utilisateur).ThenInclude(u => u.Locataire)
            .Include(r => r.Utilisateur).ThenInclude(u => u.Artisan)
            .Include(r => r.Utilisateur).ThenInclude(u => u.Agent)
            .FirstOrDefaultAsync(r => r.Token == refreshToken);

        if (token is null || !token.EstActif)
            throw new UnauthorizedAccessException("Refresh token invalide ou expiré.");

        token.DateRevocation = DateTime.UtcNow;
        var nouveauRefreshToken = await CreerRefreshTokenAsync(token.IdUser);
        await _context.SaveChangesAsync();

        var response = new AuthResponseDto
        {
            AccessToken = _jwtHelper.GenerateAccessToken(token.Utilisateur),
            User = MapToUserDto(token.Utilisateur)
        };

        return (response, nouveauRefreshToken);
    }

    public async Task LogoutAsync(string refreshToken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(r => r.Token == refreshToken);

        if (token is not null && token.DateRevocation is null)
        {
            token.DateRevocation = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    private static UserDto MapToUserDto(Utilisateur u) => new()
    {
        IdUser = u.IdUser,
        Nom = u.Nom,
        Prenom = u.Prenom,
        Email = u.Email,
        Role = u.Role.ToString(),
        KycValide = u.KycValide
    };
}

