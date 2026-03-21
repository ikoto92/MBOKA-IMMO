using MBOKA_IMMO.src.MbokaImmo.Infrastructure;
using MBOKA_IMMO.src.MbokaImmo.Infrastructure.Helpers;
using MBOKA_IMMO.src.MbokaImmo.Infrastructure.Persistence;
using MBOKA_IMMO.src.MbokaImmo.Services.Implementations;
using MBOKA_IMMO.src.MbokaImmo.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        "Host=localhost;Port=5432;Database=mboka_immo;Username=postgres;Password=123456"
    )
);

// ── Auth services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<JwtHelper>();

builder.Services.AddScoped<IUtilisateurService, UtilisateurService>();

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

// ── CORS - Correction de l'URL et ajout de AllowCredentials si nécessaire
builder.Services.AddCors(options =>
    options.AddPolicy("MbokaPolicy", policy =>
        policy.WithOrigins(
                "http://localhost:5173",
                "http://localhost:5174"  // Ajout de l'URL correcte
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()  // Important si vous utilisez l'authentification
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Ordre correct des middlewares
app.UseCors("MbokaPolicy");  // ← AJOUT OBLIGATOIRE : Appliquer la politique CORS
app.UseAuthentication();     // ← Déplacer avant UseAuthorization
app.UseAuthorization();

app.MapControllers();
app.Run();