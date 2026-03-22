using MBOKA_IMMO.src.MbokaImmo.Domain.Entities;

namespace MBOKA_IMMO.src.MbokaImmo.Data.Interfaces;

public interface IArtisanRepository
{
    Task<Artisan?> GetByIdAsync(int idArtisan);
    Task<Artisan?> GetByUserIdAsync(int idUser);
    Task<IEnumerable<Intervention>> GetInterventionsAsync(int idArtisan);
    Task<Intervention?> GetInterventionAsync(int idIntervention, int idArtisan);
    Task<IEnumerable<PaiementIntervention>> GetPaiementsAsync(int idArtisan);
    Task UpdateAsync(Artisan artisan);
    Task UpdateInterventionAsync(Intervention intervention);
    Task<int> SaveChangesAsync();
}
