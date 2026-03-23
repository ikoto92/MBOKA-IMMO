using MBOKA_IMMO.src.MbokaImmo.Data.Interfaces;
using MBOKA_IMMO.src.MbokaImmo.Data.Repositories;
using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;
using MBOKA_IMMO.src.MbokaImmo.Infrastructure.ExternalServices.Storage;
using MBOKA_IMMO.src.MbokaImmo.Infrastructure.Helpers;
using MBOKA_IMMO.src.MbokaImmo.Infrastructure.Persistence;
using MBOKA_IMMO.src.MbokaImmo.Services.Implementations;
using MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Numerics;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ── Base de données
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        "Host=localhost;Port=5432;Database=mboka_immo;Username=postgres;Password=123456"
    )
);

// ── Services métier
builder.Services.AddScoped<IBienService, BienService>();
builder.Services.AddScoped<IBienRepository, BienRepository>();
builder.Services.AddScoped<IStorageService, LocalStorageService>();
builder.Services.AddScoped<IArtisanService, ArtisanService>();
builder.Services.AddScoped<IArtisanRepository, ArtisanRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddScoped<IUtilisateurService, UtilisateurService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ICandidatureService, CandidatureService>();

// ── Upload fichiers
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600;
});
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 104857600;
});

// ── JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)
            )
        };
    });

// ── CORS
builder.Services.AddCors(options =>
    options.AddPolicy("MbokaPolicy", policy =>
        policy
            .WithOrigins(
                "http://localhost:5173",
                "http://localhost:5174",
                "https://localhost:5173",
                "https://localhost:5174"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
    )
);

var app = builder.Build(); // ← Build() d'abord

// ── Seed Admin — APRÈS builder.Build()
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!context.Utilisateurs.Any(u => u.Role == RoleEnum.Admin))
    {
        context.Utilisateurs.Add(new Utilisateur
        {
            Nom = "Admin",
            Prenom = "MBOKA",
            Email = "admin@mboka.com",
            MotDePasse = BCrypt.Net.BCrypt.HashPassword("Admin@2026!"),
            Role = RoleEnum.Admin,
            DateInscription = DateTime.UtcNow,
            CompteActif = true,
            KycValide = true,
        });
        context.SaveChanges(); // ← SaveChanges() sans await (contexte sync)
        Console.WriteLine("✅ Compte admin créé : admin@mboka.com / Admin@2026!");
    }
}

// ── Dossier uploads
var uploadsPath = Path.Combine(
    app.Environment.WebRootPath
        ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
    "uploads"
);
Directory.CreateDirectory(uploadsPath);

// ── Pipeline — ORDRE OBLIGATOIRE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MbokaPolicy");
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();