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
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ── Swagger — version complète avec CustomSchemaIds
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "MBOKA IMMO API", Version = "v1" });
    options.CustomSchemaIds(type => type.FullName?.Replace("+", "_"));
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Entrez : Bearer {token}",
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

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
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IVirementService, VirementService>();

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

var app = builder.Build();

// ── Seed Admin
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
        context.SaveChanges();
        Console.WriteLine("✅ Compte admin créé : admin@mboka.com / Admin@2026!");
    }
}

// ── Dossier uploads
var uploadsPath = Path.Combine(
    app.Environment.WebRootPath
        ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
    "uploads"
);
Directory.CreateDirectory(Path.Combine(uploadsPath, "biens"));
Directory.CreateDirectory(Path.Combine(uploadsPath, "documents"));

// ── Pipeline — Swagger HORS du if
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MBOKA IMMO API v1");
    c.RoutePrefix = "swagger";
});

app.UseCors("MbokaPolicy");
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();