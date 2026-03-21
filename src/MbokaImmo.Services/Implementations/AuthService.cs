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

    public AuthService(AppDbContext context, JwtHelper jwtHelper)
    {
        _context = context;
        _jwtHelper = jwtHelper;
    }

    // ── INSCRIPTION ──────────────────────────────────────────────
    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
    {
        // 1. Vérifier si l'email existe déjà
        var emailExiste = await _context.Utilisateurs
            .AnyAsync(u => u.Email == dto.Email);

        if (emailExiste)
            throw new InvalidOperationException("Cet email est déjà utilisé.");

        // 2. Parser le rôle
        if (!Enum.TryParse<RoleEnum>(dto.Role, true, out var role))
            throw new ArgumentException("Rôle invalide. Valeurs acceptées : Proprio, Locataire, Agent, Artisan.");

        // 3. Créer l'utilisateur
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

        // 4. Créer le profil selon le rôle
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

        // 5. Générer les tokens
        return new AuthResponseDto
        {
            AccessToken = _jwtHelper.GenerateAccessToken(utilisateur),
            RefreshToken = _jwtHelper.GenerateRefreshToken(),
            User = MapToUserDto(utilisateur)
        };
    }

    // ── CONNEXION ────────────────────────────────────────────────
    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
    {
        // 1. Trouver l'utilisateur par email
        var utilisateur = await _context.Utilisateurs
            .FirstOrDefaultAsync(u => u.Email == dto.Email && u.CompteActif);

        if (utilisateur is null)
            throw new UnauthorizedAccessException("Email ou mot de passe incorrect.");

        // 2. Vérifier le mot de passe
        if (!PasswordHelper.Verify(dto.MotDePasse, utilisateur.MotDePasse))
            throw new UnauthorizedAccessException("Email ou mot de passe incorrect.");

        // 3. Générer les tokens
        return new AuthResponseDto
        {
            AccessToken = _jwtHelper.GenerateAccessToken(utilisateur),
            RefreshToken = _jwtHelper.GenerateRefreshToken(),
            User = MapToUserDto(utilisateur)
        };
    }

    // ── Mapper ───────────────────────────────────────────────────
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
