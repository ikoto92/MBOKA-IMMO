using MBOKA_IMMO.src.MbokaImmo.Data.Interfaces;
using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;
using MBOKA_IMMO.src.MbokaImmo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MBOKA_IMMO.src.MbokaImmo.Data.Repositories;

public class ArtisanRepository(AppDbContext context) : IArtisanRepository
{
    public async Task<Artisan?> GetByIdAsync(int idArtisan)
        => await context.Artisans
            .Include(a => a.Utilisateur)
            .FirstOrDefaultAsync(a => a.IdArtisan == idArtisan);

    public async Task<Artisan?> GetByUserIdAsync(int idUser)
        => await context.Artisans
            .Include(a => a.Utilisateur)
            .FirstOrDefaultAsync(a => a.IdUser == idUser);

    public async Task<IEnumerable<Intervention>> GetInterventionsAsync(int idArtisan)
        => await context.Interventions
            .Include(i => i.Location)
                .ThenInclude(l => l.Bien)
            .Where(i => i.IdArtisan == idArtisan)
            .OrderByDescending(i => i.DateSignalement)
            .ToListAsync();

    public async Task<Intervention?> GetInterventionAsync(int idIntervention, int idArtisan)
        => await context.Interventions
            .Include(i => i.Location)
                .ThenInclude(l => l.Bien)
            .FirstOrDefaultAsync(i =>
                i.IdIntervention == idIntervention &&
                i.IdArtisan == idArtisan);

    public async Task<IEnumerable<PaiementIntervention>> GetPaiementsAsync(int idArtisan)
        => await context.PaiementsIntervention
            .Include(pi => pi.Paiement)
            .Include(pi => pi.Artisan)
            .Where(pi => pi.IdArtisan == idArtisan)
            .OrderByDescending(pi => pi.Paiement.DatePaiement)
            .ToListAsync();

    public Task UpdateAsync(Artisan artisan)
    {
        context.Artisans.Update(artisan);
        return Task.CompletedTask;
    }

    public Task UpdateInterventionAsync(Intervention intervention)
    {
        context.Interventions.Update(intervention);
        return Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync()
        => await context.SaveChangesAsync();
}
